using System.Security.Claims;
using AutoMapper;
using EduSystem.DataAccess.IRepositories;
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
        try
        {
            if (createQuizDto.SubjectId == Guid.Empty)
            {
                return ErrorResponse.Build(
                    message: "SubjectId is required",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            if (createQuizDto.TeacherId == Guid.Empty)
            {
                return ErrorResponse.Build(
                    message: "TeacherId is required",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            // Validate FK: Subject
            var subject = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == createQuizDto.SubjectId);
            if (subject == null)
            {
                return ErrorResponse.Build(
                    message: "Subject not found",
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            // Validate FK: Teacher
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.TeacherId == createQuizDto.TeacherId);
            if (teacher == null)
            {
                return ErrorResponse.Build(
                    message: "Teacher not found",
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            
            // Map data
            var quiz = _mapper.Map<CreateQuizDto, Quiz>(createQuizDto);
            quiz.SubjectId = createQuizDto.SubjectId;
            quiz.TeacherId = createQuizDto.TeacherId;
            quiz.Status = StaticOperationStatus.BaseEntity.Active;
            quiz.CreatedBy = user.FindFirstValue("Fullname");
            quiz.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.Quiz.AddAsync(quiz);
            await _unitOfWork.SaveAsync();

            return SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Created,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: quiz);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> UpdateQuiz(ClaimsPrincipal user, UpdateQuizDto updateQuizDto)
    {
        var quiz = await _unitOfWork.Quiz.GetAsync(s => s.QuizId == updateQuizDto.QuizId);
        if (quiz == null)
        {
            return SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.NotFound,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: null);
        }
        
        // Nếu Subject/Teacher có thay đổi -> validate tồn tại
        if (updateQuizDto.SubjectId != Guid.Empty && updateQuizDto.SubjectId != quiz.SubjectId)
        {
            var subject = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == updateQuizDto.SubjectId);
            if (subject == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Subject.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            quiz.SubjectId = updateQuizDto.SubjectId;
        }

        if (updateQuizDto.TeacherId != Guid.Empty && updateQuizDto.TeacherId != quiz.TeacherId)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.TeacherId == updateQuizDto.TeacherId);
            if (teacher == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            quiz.TeacherId = updateQuizDto.TeacherId;
        }
        
        var updateQuiz = _mapper.Map<UpdateQuizDto, Quiz>(updateQuizDto);
        quiz.UpdatedBy = user.FindFirstValue("Fullname");
        quiz.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        quiz.Status = updateQuiz.Status;
        
        // Update Subject
        _unitOfWork.Quiz.Update(quiz, updateQuiz);

        return (!await SaveChangesAsync()) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: updateQuiz);
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
            
            var quizzesDto = _mapper.Map<IEnumerable<Quiz>>(quizzes);

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
        return (getQuizById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Quiz.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: getQuizById);
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
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Quiz.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: deleteQuiz)
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