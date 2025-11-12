using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.QuizAttempt;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuizAttemptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateQuizAttempt(ClaimsPrincipal user, CreateQuizAttemptDto createQuizAttemptDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var student = await _unitOfWork.Student.GetAsync(s => s.StudentId == createQuizAttemptDto.StudentId);
            if (student == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Student.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == createQuizAttemptDto.QuizId);
            if (quiz == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Quiz.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var attempt = _mapper.Map<QuizAttempt>(createQuizAttemptDto);
            attempt.QuizAttemptId = Guid.NewGuid();
            attempt.Score = (decimal?)createQuizAttemptDto.Score;
            attempt.Status = StaticOperationStatus.BaseEntity.Active;
            attempt.CreatedBy = user.FindFirstValue("FullName");
            attempt.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.QuizAttempt.AddAsync(attempt);

            if (createQuizAttemptDto.StudentAnswers != null && createQuizAttemptDto.StudentAnswers.Any())
            {
                foreach (var a in createQuizAttemptDto.StudentAnswers)
                {
                    var sa = _mapper.Map<StudentAnswer>(a);
                    sa.AttemptId = attempt.QuizAttemptId;
                    sa.Status = StaticOperationStatus.BaseEntity.Active;
                    sa.CreatedBy = attempt.CreatedBy;
                    sa.CreatedTime = attempt.CreatedTime;
                    await _unitOfWork.StudentAnswer.AddAsync(sa);
                }
            }

            return (await SaveChangesAsync())
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<QuizAttemptDto>(
                        await _unitOfWork.QuizAttempt.GetAsync(
                            qa => qa.QuizAttemptId == attempt.QuizAttemptId,
                            includeProperties: "Student,Student.ApplicationUser,Quiz,StudentAnswers")))
                : ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> UpdateQuizAttempt(ClaimsPrincipal user, UpdateQuizAttemptDto updateQuizAttemptDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == updateQuizAttemptDto.QuizAttemptId &&
                      qa.Status != StaticOperationStatus.BaseEntity.Deleted,
                includeProperties: "Student,Student.ApplicationUser,Quiz,StudentAnswers");

            if (attempt == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var userRole = user.FindFirstValue(ClaimTypes.Role);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            if (student != null && attempt.StudentId != student.StudentId &&
                userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            // Update fields
            if (updateQuizAttemptDto.EndTime.HasValue) attempt.EndTime = updateQuizAttemptDto.EndTime.Value;
            if (updateQuizAttemptDto.Score.HasValue) attempt.Score = updateQuizAttemptDto.Score.Value;

            attempt.UpdatedBy = user.FindFirstValue("FullName");
            attempt.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            // Replace student answers if provided
            if (updateQuizAttemptDto.StudentAnswers != null && updateQuizAttemptDto.StudentAnswers.Any())
            {
                var existingAnswers = await _unitOfWork.StudentAnswer.GetAllAsync(sa => sa.AttemptId == attempt.QuizAttemptId);
                foreach (var old in existingAnswers)
                {
                    old.Status = StaticOperationStatus.BaseEntity.Deleted;
                    old.UpdatedBy = attempt.UpdatedBy;
                    old.UpdatedTime = attempt.UpdatedTime;
                }

                foreach (var upd in updateQuizAttemptDto.StudentAnswers)
                {
                    var sa = _mapper.Map<StudentAnswer>(upd);
                    sa.AttemptId = attempt.QuizAttemptId;
                    sa.Status = StaticOperationStatus.BaseEntity.Active;
                    sa.CreatedBy = attempt.CreatedBy;
                    sa.CreatedTime = attempt.CreatedTime;
                    sa.UpdatedBy = attempt.UpdatedBy;
                    sa.UpdatedTime = attempt.UpdatedTime;
                    await _unitOfWork.StudentAnswer.AddAsync(sa);
                }
            }

            // Return theo mẫu bạn đưa
            return (await SaveChangesAsync())
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<QuizAttemptDto>(attempt))
                : ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetQuizAttemptById(ClaimsPrincipal user, Guid quizAttemptId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == quizAttemptId &&
                      qa.Status != StaticOperationStatus.BaseEntity.Deleted,
                includeProperties: "Student,Student.ApplicationUser,Quiz,StudentAnswers");

            if (attempt == null)
                return ErrorResponse.Build(StaticResponseMessage.QuizAttempt.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);

            if (student != null && attempt.StudentId != student.StudentId &&
                userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                return ErrorResponse.Build(StaticResponseMessage.User.UnAuthorized,
                    StaticOperationStatus.StatusCode.Forbidden);

            return SuccessResponse.Build(
                StaticResponseMessage.QuizAttempt.Retrieved,
                StaticOperationStatus.StatusCode.Ok,
                _mapper.Map<QuizAttemptDto>(attempt));
        }

        public async Task<ResponseDto> GetAllQuizAttempts(
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

                var (quizAttempts, totalCount) = await _unitOfWork.QuizAttempt.GetQuizAttemptsAsync(
                    pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin, "Student,Student.ApplicationUser,Quiz");

                // Filter by student if not admin/teacher
                if (userRole == StaticUserRoles.Student)
                {
                    var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                    if (student != null)
                    {
                        quizAttempts = quizAttempts.Where(qa => qa.StudentId == student.StudentId).ToList();
                        totalCount = quizAttempts.Count;
                    }
                }

                if (quizAttempts == null || !quizAttempts.Any())
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<QuizAttempt>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.QuizAttempt.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var attemptsDto = _mapper.Map<IEnumerable<QuizAttemptDto>>(quizAttempts);

                var result = new
                {
                    Data = attemptsDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetQuizAttemptsByQuizId(ClaimsPrincipal user, Guid quizId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var role = user.FindFirstValue(ClaimTypes.Role);

                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == quizId);
                if (quiz == null)
                    return ErrorResponse.Build(StaticResponseMessage.Quiz.NotFound,
                        StaticOperationStatus.StatusCode.NotFound);

                var (attempts, total) = await _unitOfWork.QuizAttempt.GetQuizAttemptsAsync(
                    pageNumber, pageSize, "quizid", quizId.ToString(), "starttime_desc", false,
                    "Student,Student.ApplicationUser,Quiz");

                if (role == StaticUserRoles.Student)
                {
                    var stu = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                    if (stu != null)
                    {
                        attempts = attempts.Where(a => a.StudentId == stu.StudentId).ToList();
                        total = attempts.Count;
                    }
                }

                var dto = _mapper.Map<IEnumerable<QuizAttemptDto>>(attempts);

                var result = new
                {
                    Data = dto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = total,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)total / pageSize)
                };

                return SuccessResponse.Build(
                    StaticResponseMessage.QuizAttempt.Retrieved,
                    StaticOperationStatus.StatusCode.Ok,
                    result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    StaticResponseMessage.QuizAttempt.NotRetrieved + ex.Message,
                    StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetQuizAttemptsByStudentId(ClaimsPrincipal user, Guid studentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var role = user.FindFirstValue(ClaimTypes.Role);

                var student = await _unitOfWork.Student.GetAsync(s => s.StudentId == studentId);
                if (student == null)
                    return ErrorResponse.Build(StaticResponseMessage.Student.NotFound,
                        StaticOperationStatus.StatusCode.NotFound);

                var current = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                if (current != null && current.StudentId != studentId &&
                    role != StaticUserRoles.Admin && role != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(StaticResponseMessage.User.UnAuthorized,
                        StaticOperationStatus.StatusCode.Forbidden);

                var (attempts, total) = await _unitOfWork.QuizAttempt.GetQuizAttemptsAsync(
                    pageNumber, pageSize, "studentid", studentId.ToString(), "starttime_desc", false,
                    "Student,Student.ApplicationUser,Quiz");

                var dto = _mapper.Map<IEnumerable<QuizAttemptDto>>(attempts);

                var result = new
                {
                    Data = dto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = total,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)total / pageSize)
                };

                return SuccessResponse.Build(
                    StaticResponseMessage.QuizAttempt.Retrieved,
                    StaticOperationStatus.StatusCode.Ok,
                    result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    StaticResponseMessage.QuizAttempt.NotRetrieved + ex.Message,
                    StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> DeleteQuizAttempt(ClaimsPrincipal user, Guid quizAttemptId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == quizAttemptId &&
                      qa.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (attempt == null)
                return ErrorResponse.Build(StaticResponseMessage.QuizAttempt.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var role = user.FindFirstValue(ClaimTypes.Role);
            if (student != null && attempt.StudentId != student.StudentId && role != StaticUserRoles.Admin)
                return ErrorResponse.Build(StaticResponseMessage.User.UnAuthorized,
                    StaticOperationStatus.StatusCode.Forbidden);

            attempt.Status = StaticOperationStatus.BaseEntity.Deleted;
            attempt.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            attempt.UpdatedBy = user.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<QuizAttemptDto>(attempt))
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
