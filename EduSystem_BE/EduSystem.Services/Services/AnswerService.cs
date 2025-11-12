using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.Answer;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AnswerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> CreateAnswer(ClaimsPrincipal user, CreateAnswerDto createDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == createDto.QuestionId);
                if (question == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Question.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var answer = _mapper.Map<Models.Entities.Answer>(createDto);
                answer.Status = StaticOperationStatus.BaseEntity.Active;
                answer.CreatedBy = user.FindFirstValue("Fullname");
                answer.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.Answer.AddAsync(answer);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<AnswerDto>(answer));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateAnswer(ClaimsPrincipal user, UpdateAnswerDto updateDto)
        {
            try
            {
                var answer = await _unitOfWork.Answer.GetAsync(
                    a => a.AnswerId == updateDto.AnswerId
                    && a.Status != StaticOperationStatus.BaseEntity.Deleted);

                if (answer == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Answer.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Lấy QuestionId mục tiêu: nếu không gửi hoặc Guid.Empty => giữ nguyên
                var targetQuestionId = (updateDto.QuestionId.HasValue && updateDto.QuestionId.Value != Guid.Empty)
                    ? updateDto.QuestionId.Value
                    : answer.QuestionId;

                // Nếu có thay đổi QuestionId thì kiểm tra tồn tại
                if (targetQuestionId != answer.QuestionId)
                {
                    var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == targetQuestionId);
                    if (question == null)
                        return ErrorResponse.Build(
                            message: StaticResponseMessage.Question.NotFound,
                            statusCode: StaticOperationStatus.StatusCode.BadRequest);
                }

                // Cập nhật trực tiếp để tránh reset FK
                answer.Content = updateDto.Content;
                answer.IsCorrect = updateDto.IsCorrect;
                answer.Explanation = updateDto.Explanation;
                answer.QuestionId = targetQuestionId;

                answer.UpdatedBy = user.FindFirstValue("Fullname");
                answer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                _unitOfWork.Answer.Update(answer);

                if (!await SaveChangesAsync())
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Answer.NotUpdated,
                        statusCode: StaticOperationStatus.StatusCode.InternalServerError);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<AnswerDto>(answer));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAllAnswers(
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

                var (answers, totalAnswers) = await _unitOfWork.Answer
                    .GetAnswersAsync(pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

                if (answers == null || !answers.Any() || totalAnswers == 0)
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<Models.Entities.Answer>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.Answer.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var answersDto = _mapper.Map<List<AnswerDto>>(answers);
                var result = new
                {
                    Data = answersDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalAnswers,
                    TotalPages = (int)Math.Ceiling((double)totalAnswers / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalAnswers / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAnswerById(ClaimsPrincipal user, Guid answerId)
        {
            try
            {
                var answer = await _unitOfWork.Answer.GetAsync(
                    a => a.AnswerId == answerId
                    && a.Status != StaticOperationStatus.BaseEntity.Deleted);

                if (answer == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Answer.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<AnswerDto>(answer));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotFound + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAnswersByQuestionId(ClaimsPrincipal user, Guid questionId)
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

                var answers = await _unitOfWork.Answer.GetAnswersByQuestionId(questionId);
                if (!answers.Any())
                    return SuccessResponse.Build(
                        message: StaticResponseMessage.Answer.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: new List<AnswerDto>());

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<List<AnswerDto>>(answers));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> DeleteAnswer(ClaimsPrincipal user, Guid answerId)
        {
            try
            {
                var answer = await _unitOfWork.Answer.GetAsync(
                    a => a.AnswerId == answerId
                    && a.Status != StaticOperationStatus.BaseEntity.Deleted);

                if (answer == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Answer.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                answer.Status = StaticOperationStatus.BaseEntity.Deleted;
                answer.UpdatedBy = user.FindFirstValue("Fullname");
                answer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                if (!await SaveChangesAsync())
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Answer.NotDeleted,
                        statusCode: StaticOperationStatus.StatusCode.InternalServerError);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Answer.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<AnswerDto>(answer));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Answer.NotDeleted + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
