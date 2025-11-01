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
        try
        {
            if (createQuestionDto.SubjectId == Guid.Empty)
            {
                return ErrorResponse.Build(
                    message: "SubjectId is required",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            if (createQuestionDto.TeacherId == Guid.Empty)
            {
                return ErrorResponse.Build(
                    message: "TeacherId is required",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            if (createQuestionDto.LessonId == Guid.Empty)
            {
                return ErrorResponse.Build(
                    message: "LessonId is required",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            // Validate FK: Teacher
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.TeacherId == createQuestionDto.TeacherId);
            if (teacher == null)
            {
                return ErrorResponse.Build(
                    message: "Teacher not found",
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            // Map data
            var question = _mapper.Map<CreateQuestionDto, Question>(createQuestionDto);
            question.TeacherId = createQuestionDto.TeacherId;
            question.Status = StaticOperationStatus.BaseEntity.Active;
            question.CreatedBy = user.FindFirstValue("Fullname");
            question.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.Question.AddAsync(question);
            await _unitOfWork.SaveAsync();

            return SuccessResponse.Build(
                message: StaticResponseMessage.Question.Created,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: question);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> UpdateQuestion(ClaimsPrincipal user, UpdateQuestionDto updateQuestionDto)
    {
        var question =  await _unitOfWork.Question.GetAsync(s => s.QuestionId == updateQuestionDto.QuestionId);
        if (question == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Nếu Subject/Teacher/Lesson có thay đổi -> validate tồn tại
        
        if (updateQuestionDto.TeacherId != Guid.Empty && updateQuestionDto.TeacherId != question.TeacherId)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(s => s.TeacherId == updateQuestionDto.TeacherId);
            if (teacher == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            question.TeacherId = updateQuestionDto.TeacherId;
        }
        
        // Map data
        var updateQuestion = _mapper.Map<UpdateQuestionDto, Question>(updateQuestionDto);
        question.UpdatedBy = user.FindFirstValue("Fullname");
        question.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        question.Status = updateQuestion.Status;
        
        // Update Question
        _unitOfWork.Question.Update(question, updateQuestion);
        
        return (!await SaveChangesAsync()) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: updateQuestion);
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
            
            var questionDto = _mapper.Map<IEnumerable<Question>>(questions);

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
        return (getQuestionById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: getQuestionById);
    }

    public async Task<ResponseDto> DeleteQuestion(ClaimsPrincipal user, Guid questionId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
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

        deleteQuestion.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteQuestion.UpdatedBy = user.FindFirstValue("Fullname");
        deleteQuestion.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Question.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: deleteQuestion)
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