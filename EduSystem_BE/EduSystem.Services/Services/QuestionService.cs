using AutoMapper;
using EduSystem.Repository.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

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
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
            if (teacher == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Create question
            var question = _mapper.Map<Question>(createQuestionDto);
            question.TeacherId = teacher.TeacherId;
            question.Status = StaticOperationStatus.BaseEntity.Active;
            question.CreatedBy = user.FindFirstValue("Fullname");
            question.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.Question.AddAsync(question);

            // Create answers if provided
            if (createQuestionDto.Answers != null && createQuestionDto.Answers.Any())
            {
                foreach (var answerDto in createQuestionDto.Answers)
                {
                    var answer = _mapper.Map<Answer>(answerDto);
                    answer.QuestionId = question.QuestionId;
                    answer.Status = StaticOperationStatus.BaseEntity.Active;
                    answer.CreatedBy = user.FindFirstValue("Fullname");
                    answer.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
                    await _unitOfWork.Answer.AddAsync(answer);
                }
            }

            await _unitOfWork.SaveAsync();

            // Get question with answers for response
            var resultQuestion = await _unitOfWork.Question
                .GetAsync(q => q.QuestionId == question.QuestionId,
                         includeProperties: "Answers");

            return SuccessResponse.Build(
                message: StaticResponseMessage.Question.Created,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: _mapper.Map<QuestionDto>(resultQuestion));
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
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
            q => q.QuestionId == updateQuestionDto.QuestionId
            && q.Status != StaticOperationStatus.BaseEntity.Deleted,
            includeProperties: "Answers");

            if (question == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Question.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Update question
            var updateQuestion = _mapper.Map<Question>(updateQuestionDto);
            updateQuestion.TeacherId = question.TeacherId;
            updateQuestion.Status = question.Status;
            updateQuestion.CreatedBy = question.CreatedBy;
            updateQuestion.CreatedTime = question.CreatedTime;
            updateQuestion.UpdatedBy = user.FindFirstValue("Fullname");
            updateQuestion.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            _unitOfWork.Question.Update(question, updateQuestion);

            // Update answers
            if (updateQuestionDto.Answers != null)
            {
                // Delete removed answers
                var existingAnswerIds = updateQuestionDto.Answers
                    .Where(a => a.AnswerId != Guid.Empty)
                    .Select(a => a.AnswerId);
                var answersToDelete = question.Answers
                    .Where(a => !existingAnswerIds.Contains(a.AnswerId));
                foreach (var answer in answersToDelete)
                {
                    answer.Status = StaticOperationStatus.BaseEntity.Deleted;
                    answer.UpdatedBy = user.FindFirstValue("Fullname");
                    answer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
                }

                // Update existing and add new answers
                foreach (var answerDto in updateQuestionDto.Answers)
                {
                    if (answerDto.AnswerId == Guid.Empty)
                    {
                        // New answer
                        var newAnswer = _mapper.Map<Answer>(answerDto);
                        newAnswer.QuestionId = question.QuestionId;
                        newAnswer.Status = StaticOperationStatus.BaseEntity.Active;
                        newAnswer.CreatedBy = user.FindFirstValue("Fullname");
                        newAnswer.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
                        await _unitOfWork.Answer.AddAsync(newAnswer);
                    }
                    else
                    {
                        // Update existing answer
                        var existingAnswer = question.Answers
                            .FirstOrDefault(a => a.AnswerId == answerDto.AnswerId);
                        if (existingAnswer != null)
                        {
                            var updateAnswer = _mapper.Map<Answer>(answerDto);
                            updateAnswer.QuestionId = question.QuestionId;
                            updateAnswer.Status = existingAnswer.Status;
                            updateAnswer.CreatedBy = existingAnswer.CreatedBy;
                            updateAnswer.CreatedTime = existingAnswer.CreatedTime;
                            updateAnswer.UpdatedBy = user.FindFirstValue("Fullname");
                            updateAnswer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
                            _unitOfWork.Answer.Update(existingAnswer, updateAnswer);
                        }
                    }
                }
            }

            if (!await SaveChangesAsync())
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Question.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);

            var resultQuestion = await _unitOfWork.Question
                .GetAsync(q => q.QuestionId == question.QuestionId,
                         includeProperties: "Answers");

            return SuccessResponse.Build(
                message: StaticResponseMessage.Question.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: _mapper.Map<QuestionDto>(resultQuestion));
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Question.NotUpdated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
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

    public async Task<ResponseDto> GetQuestionsByCurrentTeacher(
        ClaimsPrincipal User,
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
            if (teacher == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var (questions, totalQuestions) = await _unitOfWork.Question.GetQuestionsByCurrentTeacherAsync(
                teacher.TeacherId, pageNumber, pageSize, filterOn, filterQuery, sortBy);

            if (questions == null || !questions.Any() || totalQuestions == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<QuestionDto>(),
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

            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            var result = new
            {
                Data = questionDtos,
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