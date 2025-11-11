using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.QuizQuestion;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class QuizQuestionService : IQuizQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuizQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddQuestionToQuiz(ClaimsPrincipal user, CreateQuizQuestionDto createQuizQuestionDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == createQuizQuestionDto.QuizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == createQuizQuestionDto.QuestionId);
                if (question == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Question.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var existingQuizQuestion = await _unitOfWork.QuizQuestion
                    .IsQuestionInQuizAsync(createQuizQuestionDto.QuizId, createQuizQuestionDto.QuestionId);

                if (existingQuizQuestion)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizQuestion.AlreadyExist,
                        statusCode: StaticOperationStatus.StatusCode.BadRequest);

                // ✅ TỰ ĐỘNG TẠO QuestionOrder nếu = 0
                if (createQuizQuestionDto.QuestionOrder <= 0)
                {
                    var maxOrder = await _unitOfWork.QuizQuestion.GetMaxQuestionOrderAsync(createQuizQuestionDto.QuizId);
                    createQuizQuestionDto.QuestionOrder = maxOrder + 1;
                }

                var quizQuestion = _mapper.Map<QuizQuestion>(createQuizQuestionDto);
                quizQuestion.Status = StaticOperationStatus.BaseEntity.Active;
                quizQuestion.CreatedBy = user.FindFirstValue("FullName") ?? "System";
                quizQuestion.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.QuizQuestion.AddAsync(quizQuestion);
                await _unitOfWork.SaveAsync();

                var createdQuizQuestion = await _unitOfWork.QuizQuestion.GetAsync(
                    qq => qq.QuizId == quizQuestion.QuizId && qq.QuestionId == quizQuestion.QuestionId,
                    includeProperties: "Quiz,Question");

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<QuizQuestionDto>(createdQuizQuestion));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> AddMultipleQuestionsToQuiz(ClaimsPrincipal user, AddQuestionsToQuizDto addQuestionsToQuizDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == addQuestionsToQuizDto.QuizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // ✅ TỰ ĐỘNG LẤY MAX ORDER
                var maxOrder = await _unitOfWork.QuizQuestion.GetMaxQuestionOrderAsync(addQuestionsToQuizDto.QuizId);
                var currentOrder = maxOrder;

                var quizQuestions = new List<QuizQuestion>();
                var addedCount = 0;
                var skippedCount = 0;

                foreach (var questionId in addQuestionsToQuizDto.QuestionIds)
                {
                    var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == questionId);
                    if (question == null)
                    {
                        skippedCount++;
                        continue;
                    }

                    var exists = await _unitOfWork.QuizQuestion
                        .IsQuestionInQuizAsync(addQuestionsToQuizDto.QuizId, questionId);

                    if (exists)
                    {
                        skippedCount++;
                        continue;
                    }

                    // ✅ TỰ ĐỘNG TĂNG ORDER
                    currentOrder++;
                    var quizQuestion = new QuizQuestion
                    {
                        QuizId = addQuestionsToQuizDto.QuizId,
                        QuestionId = questionId,
                        QuestionOrder = currentOrder,
                        Status = StaticOperationStatus.BaseEntity.Active,
                        CreatedBy = user.FindFirstValue("FullName") ?? "System",
                        CreatedTime = StaticOperationStatus.Timezone.Vietnam
                    };

                    quizQuestions.Add(quizQuestion);
                    addedCount++;
                }

                if (quizQuestions.Any())
                {
                    await _unitOfWork.QuizQuestion.AddRangeAsync(quizQuestions);
                    await _unitOfWork.SaveAsync();
                }

                var result = new
                {
                    QuizId = addQuestionsToQuizDto.QuizId,
                    TotalRequested = addQuestionsToQuizDto.QuestionIds.Count,
                    Added = addedCount,
                    Skipped = skippedCount
                };

                return SuccessResponse.Build(
                    message: $"{addedCount} question(s) added successfully. {skippedCount} skipped.",
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetQuestionsByQuizId(ClaimsPrincipal user, Guid quizId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == quizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var quizQuestions = await _unitOfWork.QuizQuestion.GetQuestionsByQuizIdAsync(quizId);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<IEnumerable<QuizQuestionDto>>(quizQuestions));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetQuizzesByQuestionId(ClaimsPrincipal user, Guid questionId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == questionId);
                if (question == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Question.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var quizQuestions = await _unitOfWork.QuizQuestion.GetQuizzesByQuestionIdAsync(questionId);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<IEnumerable<QuizQuestionDto>>(quizQuestions));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateQuestionOrder(ClaimsPrincipal user, UpdateQuestionOrderDto updateQuestionOrderDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quizQuestion = await _unitOfWork.QuizQuestion
                    .GetAsync(qq => qq.QuizId == updateQuestionOrderDto.QuizId && qq.QuestionId == updateQuestionOrderDto.QuestionId);

                if (quizQuestion == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizQuestion.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                quizQuestion.QuestionOrder = updateQuestionOrderDto.NewOrder;
                quizQuestion.UpdatedBy = user.FindFirstValue("FullName") ?? "System";
                quizQuestion.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                _unitOfWork.QuizQuestion.Update(quizQuestion);
                await _unitOfWork.SaveAsync();

                var updatedQuizQuestion = await _unitOfWork.QuizQuestion.GetAsync(
                    qq => qq.QuizId == updateQuestionOrderDto.QuizId && qq.QuestionId == updateQuestionOrderDto.QuestionId,
                    includeProperties: "Quiz,Question");

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<QuizQuestionDto>(updatedQuizQuestion));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> RemoveQuestionFromQuiz(ClaimsPrincipal user, Guid quizId, Guid questionId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quizQuestion = await _unitOfWork.QuizQuestion
                    .GetAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

                if (quizQuestion == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizQuestion.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                _unitOfWork.QuizQuestion.Remove(quizQuestion);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotDeleted + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> RemoveAllQuestionsFromQuiz(ClaimsPrincipal user, Guid quizId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == quizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                await _unitOfWork.QuizQuestion.RemoveQuestionsByQuizIdAsync(quizId);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: "All questions removed from quiz successfully",
                    statusCode: StaticOperationStatus.StatusCode.Ok);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizQuestion.NotDeleted + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

    }
}
