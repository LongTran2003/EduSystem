using System.Security.Claims;
using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class QuestionService : IQuestionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public QuestionService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ResponseDto> CreateQuestion(ClaimsPrincipal user, CreateQuestionDto createQuestionDto)
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
            
            var question = _mapper.Map<CreateQuestionDto, Question>(createQuestionDto);
            question.TeacherId = teacher.TeacherId;
            
            var existQuestion = await _unitOfWork.Question.GetAsync(q => q.TeacherId == question.TeacherId 
                                                                         && q.Content == question.Content);
            if (existQuestion != null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Question.AlreadyExist,
                    statusCode: StaticOperationStatus.StatusCode.Conflict); 
            }
            
            question.CreatedBy = user.FindFirstValue("FullName");
            question.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
            question.Status = StaticOperationStatus.BaseEntity.Active;

            try
            {
                await _unitOfWork.Question.AddAsync(question);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            var resultDto = _mapper.Map<QuestionDto>(question); 
            
            return SuccessResponse.Build(
                message: StaticResponseMessage.Question.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
    }

    public async Task<ResponseDto> UpdateQuestion(ClaimsPrincipal user, UpdateQuestionDto updateQuestionDto)
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
        
        var question =  await _unitOfWork.Question.GetAsync(s => s.QuestionId == updateQuestionDto.QuestionId,
            includeProperties: "Teacher.ApplicationUser");
        if (question == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotFound,
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
        
        // Map data
        var updateQuestion = _mapper.Map<UpdateQuestionDto, Question>(updateQuestionDto);
        updateQuestion.TeacherId = teacher.TeacherId; // Đảm bảo TeacherId được giữ nguyên
        updateQuestion.UpdatedBy = user.FindFirstValue("Fullname");
        updateQuestion.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        updateQuestion.Status = updateQuestion.Status;
        updateQuestion.CreatedBy = question.CreatedBy;
        updateQuestion.CreatedTime = question.CreatedTime;
        
        // Update Question
        _unitOfWork.Question.Update(question, updateQuestion);
        
        var resultDto =  _mapper.Map<QuestionDto>(question);
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }

    public async Task<ResponseDto> GetAllQuestions
    (
        ClaimsPrincipal user, 
        int pageNumber = 1, 
        int pageSize = 10, 
        string? filterOn = null,
        string? filterQuery = null, 
        string? sortBy = null)
    {
        try
        {
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            bool isAdmin = userRole == StaticUserRoles.Admin;

            var (questions, totalQuestions) = await _unitOfWork.Question.GetQuestionsAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (questions == null || !questions.Any() || totalQuestions == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<Question>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Question.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }
            
            var questionDto = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            var result = new
            {
                Data = questionDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalQuestions,
                TotalPages = (int)Math.Ceiling((double)totalQuestions / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalQuestions / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.Question.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> GetQuestionById(ClaimsPrincipal user, Guid questionId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        var getQuestionById = await _unitOfWork.Question.GetAsync(s => s.QuestionId == questionId 
                                                               && s.Status != StaticOperationStatus.BaseEntity.Deleted);
        var resultDto = _mapper.Map<QuestionDto>(getQuestionById);
        
        return (getQuestionById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
    }

    public async Task<ResponseDto> DeleteQuestion(ClaimsPrincipal user, Guid questionId)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var deleteQuestion = await _unitOfWork.Question.GetAsync(s => s.QuestionId == questionId
                                                              &&  s.Status != StaticOperationStatus.BaseEntity.Deleted);
        if (deleteQuestion is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Kiểm tra xem người dùng hiện tại có phải là giáo viên sở hữu câu hỏi này không
        var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
        if (teacher == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Teacher.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Chỉ cho phép giáo viên sở hữu câu hỏi hoặc admin xóa câu hỏi
        var isAdmin = user.FindFirstValue(ClaimTypes.Role) == StaticUserRoles.Admin;
        if (deleteQuestion.TeacherId != teacher.TeacherId && !isAdmin)
        {
            return ErrorResponse.Build(
                message: "Bạn không có quyền xóa câu hỏi này",
                statusCode: StaticOperationStatus.StatusCode.Forbidden);
        }

        deleteQuestion.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteQuestion.UpdatedBy = user.FindFirstValue("Fullname");
        deleteQuestion.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        var resultDto =  _mapper.Map<QuestionDto>(deleteQuestion);
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}