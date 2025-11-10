using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.StudentAnswers;
using EduSystem.Models.DTOs.StudentAnswers.SubmitAnswer;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class StudentAnswerService : IStudentAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentAnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateStudentAnswer(ClaimsPrincipal user, CreateStudentAnswerDto createStudentAnswerDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                // Validate attempt exists and belongs to student
                var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                    qa => qa.QuizAttemptId == createStudentAnswerDto.AttemptId,
                    includeProperties: "Student");

                if (attempt == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizAttempt.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && attempt.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                // Validate question exists
                var question = await _unitOfWork.Question.GetAsync(q => q.QuestionId == createStudentAnswerDto.QuestionId);
                if (question == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Question.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Validate answer if provided
                if (createStudentAnswerDto.AnswerId.HasValue)
                {
                    var answer = await _unitOfWork.Answer.GetAsync(a => a.AnswerId == createStudentAnswerDto.AnswerId.Value);
                    if (answer == null)
                        return ErrorResponse.Build(
                            message: StaticResponseMessage.Answer.NotFound,
                            statusCode: StaticOperationStatus.StatusCode.NotFound);
                }

                // Check if answer already exists
                var existingAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(
                    createStudentAnswerDto.AttemptId, createStudentAnswerDto.QuestionId);

                if (existingAnswer != null)
                    return ErrorResponse.Build(
                        message: "Answer already exists for this question. Use update instead.",
                        statusCode: StaticOperationStatus.StatusCode.BadRequest);

                var studentAnswer = _mapper.Map<StudentAnswer>(createStudentAnswerDto);
                studentAnswer.Status = StaticOperationStatus.BaseEntity.Active;
                studentAnswer.CreatedBy = user.FindFirstValue("FullName");
                studentAnswer.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.StudentAnswer.AddAsync(studentAnswer);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<StudentAnswerDto>(studentAnswer));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> SubmitAnswers(ClaimsPrincipal user, SubmitAnswersDto submitAnswersDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                // Validate attempt
                var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                    qa => qa.QuizAttemptId == submitAnswersDto.AttemptId,
                    includeProperties: "Student");

                if (attempt == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizAttempt.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                if (student == null || attempt.StudentId != student.StudentId)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                var fullName = user.FindFirstValue("FullName");
                var currentTime = StaticOperationStatus.Timezone.Vietnam;

                foreach (var answerSubmission in submitAnswersDto.Answers)
                {
                    // Check if answer already exists
                    var existingAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(
                        submitAnswersDto.AttemptId, answerSubmission.QuestionId);

                    if (existingAnswer != null)
                    {
                        // Update existing answer
                        existingAnswer.AnswerId = answerSubmission.AnswerId;
                        existingAnswer.Answers = answerSubmission.Answers;
                        existingAnswer.UpdatedBy = fullName;
                        existingAnswer.UpdatedTime = currentTime;
                    }
                    else
                    {
                        // Create new answer
                        var studentAnswer = new StudentAnswer
                        {
                            AttemptId = submitAnswersDto.AttemptId,
                            QuestionId = answerSubmission.QuestionId,
                            AnswerId = answerSubmission.AnswerId,
                            Answers = answerSubmission.Answers,
                            Status = StaticOperationStatus.BaseEntity.Active,
                            CreatedBy = fullName,
                            CreatedTime = currentTime
                        };

                        await _unitOfWork.StudentAnswer.AddAsync(studentAnswer);
                    }
                }

                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: "Answers submitted successfully",
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: new { SubmittedCount = submitAnswersDto.Answers.Count });
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: "Failed to submit answers: " + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateStudentAnswer(ClaimsPrincipal user, UpdateStudentAnswerDto updateStudentAnswerDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var updateStudentAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(
                updateStudentAnswerDto.AttemptId, updateStudentAnswerDto.QuestionId);

            if (updateStudentAnswer is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify ownership
            var attempt = await _unitOfWork.QuizAttempt.GetAsync(qa => qa.QuizAttemptId == updateStudentAnswerDto.AttemptId);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            if (student != null && attempt!.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            var updatedStudentAnswer = _mapper.Map<UpdateStudentAnswerDto, StudentAnswer>(updateStudentAnswerDto);
            updatedStudentAnswer.Status = StaticOperationStatus.BaseEntity.Active;
            updatedStudentAnswer.CreatedBy = updateStudentAnswer.CreatedBy;
            updatedStudentAnswer.CreatedTime = updateStudentAnswer.CreatedTime;
            updatedStudentAnswer.UpdatedBy = user.FindFirstValue("FullName");
            updatedStudentAnswer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            _unitOfWork.StudentAnswer.Update(updateStudentAnswer, updatedStudentAnswer);

            var resultDto = _mapper.Map<StudentAnswerDto>(updateStudentAnswer);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetStudentAnswerById(ClaimsPrincipal user, Guid attemptId, Guid questionId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var getStudentAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(attemptId, questionId);

            if (getStudentAnswer is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify access rights
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var attempt = await _unitOfWork.QuizAttempt.GetAsync(qa => qa.QuizAttemptId == attemptId);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            if (student != null && attempt!.StudentId != student.StudentId &&
                userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            var resultDto = _mapper.Map<StudentAnswerDto>(getStudentAnswer);

            return SuccessResponse.Build(
                message: StaticResponseMessage.StudentAnswer.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
        }

        public async Task<ResponseDto> GetAllStudentAnswers(
            ClaimsPrincipal user,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = user.FindFirstValue(ClaimTypes.Role);
                bool isAdmin = userRole == StaticUserRoles.Admin;

                var (studentAnswers, totalCount) = await _unitOfWork.StudentAnswer.GetStudentAnswersAsync(
                    pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin, "Question,Answer,QuizAttempt");

                if (studentAnswers == null || !studentAnswers.Any())
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<StudentAnswer>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.StudentAnswer.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var answersDto = _mapper.Map<IEnumerable<StudentAnswerDto>>(studentAnswers);

                var result = new
                {
                    Data = answersDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAnswersByAttemptId(ClaimsPrincipal user, Guid attemptId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                // Verify attempt exists
                var attempt = await _unitOfWork.QuizAttempt.GetAsync(qa => qa.QuizAttemptId == attemptId);
                if (attempt == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.QuizAttempt.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Verify access rights
                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && attempt.StudentId != student.StudentId &&
                    userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                var answers = await _unitOfWork.StudentAnswer.GetAnswersByAttemptIdAsync(attemptId);
                var answersDto = _mapper.Map<IEnumerable<StudentAnswerDto>>(answers);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: answersDto);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GradeAnswer(ClaimsPrincipal user, UpdateStudentAnswerDto gradeDto)
        {
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            if (userRole != StaticUserRoles.Teacher && userRole != StaticUserRoles.Admin)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            var existingAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(
                gradeDto.AttemptId, gradeDto.QuestionId);

            if (existingAnswer is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            existingAnswer.IsCorrect = gradeDto.IsCorrect;
            existingAnswer.Score = gradeDto.Score;
            existingAnswer.TeacherFeedback = gradeDto.TeacherFeedback;
            existingAnswer.UpdatedBy = user.FindFirstValue("FullName");
            existingAnswer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            var resultDto = _mapper.Map<StudentAnswerDto>(existingAnswer);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: "Answer graded successfully",
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto)
                :
                ErrorResponse.Build(
                    message: "Failed to grade answer",
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> DeleteStudentAnswer(ClaimsPrincipal user, Guid attemptId, Guid questionId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var deleteStudentAnswer = await _unitOfWork.StudentAnswer.GetAnswerByAttemptAndQuestionAsync(attemptId, questionId);

            if (deleteStudentAnswer is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify ownership
            var attempt = await _unitOfWork.QuizAttempt.GetAsync(qa => qa.QuizAttemptId == attemptId);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            if (student != null && attempt!.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            deleteStudentAnswer.Status = StaticOperationStatus.BaseEntity.Deleted;
            deleteStudentAnswer.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteStudentAnswer.UpdatedBy = user.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<StudentAnswerDto>(deleteStudentAnswer))
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.StudentAnswer.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }

    }
}
