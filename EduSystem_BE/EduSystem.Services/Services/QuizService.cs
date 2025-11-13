using System.Security.Claims;
using AutoMapper;
using EduSystem.Repository.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Quiz;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class QuizService : IQuizService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ResponseDto> CreateQuiz(ClaimsPrincipal user, CreateQuizDto createQuizDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Lấy TeacherId từ người dùng hiện tại
        var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
        if (teacher == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Teacher.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Kiểm tra Matrix có tồn tại không
        var matrix = await _unitOfWork.Matrix.GetAsync(m => m.MatrixId == createQuizDto.MatrixId);
        if (matrix == null)
        {
            return ErrorResponse.Build(
                message: "Matrix not found. Please create a matrix first.",
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Map dữ liệu qua Dto và gán TeacherId từ người dùng hiện tại
        var createQuiz = _mapper.Map<CreateQuizDto, Quiz>(createQuizDto);
        createQuiz.TeacherId = teacher.TeacherId;
        
        // Check xem QuizName đã tồn tại với gvien đó chưa
        var existingUnit = await _unitOfWork.Quiz.GetAsync(u =>
                u.TeacherId == createQuiz.TeacherId &&
                u.QuizName == createQuiz.QuizName
        );

        if (existingUnit != null)
        {
            // Nếu tìm thấy, tức là đã tồn tại -> Trả về lỗi
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.AlreadyExist,
                statusCode: StaticOperationStatus.StatusCode.Conflict); // 409 Conflict là mã lỗi phù hợp
        }
        
        // Cập nhật lại dữ liệu BaseEntity
        createQuiz.CreatedBy = user.FindFirstValue("FullName");
        createQuiz.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
        createQuiz.Status = StaticOperationStatus.BaseEntity.Active;
        
        try
        {
            await _unitOfWork.Quiz.AddAsync(createQuiz);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
        
        var resultDto = _mapper.Map<QuizDto>(createQuiz);
        
        return SuccessResponse.Build(
            message: StaticResponseMessage.Unit.Created,
            statusCode: StaticOperationStatus.StatusCode.Ok,
            result: resultDto);
    }

    public async Task<ResponseDto> UpdateQuiz(ClaimsPrincipal user, UpdateQuizDto updateQuizDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var userTeacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
        if (userTeacher is null)
        {
            return ErrorResponse.Build(
                message: "User is not a valid teacher.",
                statusCode: StaticOperationStatus.StatusCode.Forbidden); // 403 Forbidden
        }
        
        // Kiểm tra Matrix có tồn tại không
        var matrix = await _unitOfWork.Matrix.GetAsync(m => m.MatrixId == updateQuizDto.MatrixId);
        if (matrix == null)
        {
            return ErrorResponse.Build(
                message: "Matrix not found. Please select a valid matrix.",
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        var updateQuiz = await _unitOfWork.Quiz.GetAsync(u => u.QuizId == updateQuizDto.QuizId,
            includeProperties: "Teacher.ApplicationUser");
        if (updateQuiz is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        // Preserve the TeacherId from the existing unit
        var updatedQuiz = _mapper.Map<UpdateQuizDto, Quiz>(updateQuizDto);
        updatedQuiz.TeacherId = updateQuiz.TeacherId; // Keep the original TeacherId
        updatedQuiz.Status = StaticOperationStatus.BaseEntity.Active;
        updatedQuiz.CreatedBy = updateQuiz.CreatedBy;
        updatedQuiz.CreatedTime = updateQuiz.CreatedTime;
        updatedQuiz.UpdatedBy = user.FindFirstValue("FullName");
        updatedQuiz.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        _unitOfWork.Quiz.Update(updateQuiz, updatedQuiz);
        
        var resultDto =  _mapper.Map<QuizDto>(updateQuiz);
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }

    public async Task<ResponseDto> GetAllQuizzes
    (
        ClaimsPrincipal user, 
        int pageNumber = 1, 
        int pageSize = 10, 
        string? filterOn = null,
        string? filterQuery = null, 
        string? sortBy = null
        )
    {
        try
        {
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            bool isAdmin = userRole == StaticUserRoles.Admin;

            var (quizzes, totalQuizzes) = await _unitOfWork.Quiz.GetQuizzesAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (quizzes == null || !quizzes.Any() || totalQuizzes == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<Quiz>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Quiz.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }
            
            var quizzesDto = _mapper.Map<IEnumerable<QuizDto>>(quizzes);

            var result = new
            {
                Data = quizzesDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalQuizzes,
                TotalPages = (int)Math.Ceiling((double)totalQuizzes / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalQuizzes / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> GetQuizById(ClaimsPrincipal user, Guid quizId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        var getQuizById = await _unitOfWork.Quiz.GetAsync(s => s.QuizId == quizId 
                                                                   && s.Status != StaticOperationStatus.BaseEntity.Deleted);
        
        var resultUnitDto = _mapper.Map<QuizDto>(getQuizById); 
        
        return (getQuizById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultUnitDto);
    }

    public async Task<ResponseDto> DeleteQuiz(ClaimsPrincipal user, Guid quizId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var deleteQuiz = await _unitOfWork.Quiz.GetAsync(s => s.QuizId == quizId
                                                                  &&  s.Status != StaticOperationStatus.BaseEntity.Deleted);
        if (deleteQuiz is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        deleteQuiz.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteQuiz.UpdatedBy = user.FindFirstValue("Fullname");
        deleteQuiz.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        var resultDto = _mapper.Map<QuizDto>(deleteQuiz);
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}