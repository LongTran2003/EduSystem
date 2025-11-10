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
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                // Validate student exists
                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                if (student == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Student.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Validate quiz exists
                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == createQuizAttemptDto.QuizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var quizAttempt = _mapper.Map<QuizAttempt>(createQuizAttemptDto);
                quizAttempt.QuizAttemptId = Guid.NewGuid();
                quizAttempt.StudentId = student.StudentId;
                quizAttempt.Status = StaticOperationStatus.BaseEntity.Active;
                quizAttempt.CreatedBy = user.FindFirstValue("FullName");
                quizAttempt.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.QuizAttempt.AddAsync(quizAttempt);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<QuizAttemptDto>(quizAttempt));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateQuizAttempt(ClaimsPrincipal user, UpdateQuizAttemptDto updateQuizAttemptDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var updateQuizAttempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == updateQuizAttemptDto.QuizAttemptId &&
                qa.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (updateQuizAttempt is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify ownership (student can only update their own attempts)
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            if (student != null && updateQuizAttempt.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            var updatedQuizAttempt = _mapper.Map<UpdateQuizAttemptDto, QuizAttempt>(updateQuizAttemptDto);
            updatedQuizAttempt.Status = StaticOperationStatus.BaseEntity.Active;
            updatedQuizAttempt.CreatedBy = updateQuizAttempt.CreatedBy;
            updatedQuizAttempt.CreatedTime = updateQuizAttempt.CreatedTime;
            updatedQuizAttempt.UpdatedBy = user.FindFirstValue("FullName");
            updatedQuizAttempt.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            _unitOfWork.QuizAttempt.Update(updateQuizAttempt, updatedQuizAttempt);

            var resultDto = _mapper.Map<QuizAttemptDto>(updateQuizAttempt);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetQuizAttemptById(ClaimsPrincipal user, Guid quizAttemptId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var getQuizAttempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == quizAttemptId &&
                qa.Status != StaticOperationStatus.BaseEntity.Deleted,
                includeProperties: "Student,Quiz,StudentAnswers");

            if (getQuizAttempt is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify access rights
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            if (student != null && getQuizAttempt.StudentId != student.StudentId &&
                userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            var resultDto = _mapper.Map<QuizAttemptDto>(getQuizAttempt);

            return SuccessResponse.Build(
                message: StaticResponseMessage.QuizAttempt.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
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
                    pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin, "Student,Quiz");

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
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                // Verify quiz exists
                var quiz = await _unitOfWork.Quiz.GetAsync(q => q.QuizId == quizId);
                if (quiz == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Quiz.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var (quizAttempts, totalCount) = await _unitOfWork.QuizAttempt.GetQuizAttemptsAsync(
                    pageNumber, pageSize, "quizid", quizId.ToString(), "starttime_desc", false, "Student");

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

        public async Task<ResponseDto> GetQuizAttemptsByStudentId(ClaimsPrincipal user, Guid studentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                // Verify student exists
                var student = await _unitOfWork.Student.GetAsync(s => s.StudentId == studentId);
                if (student == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Student.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Verify access rights
                var currentStudent = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                if (currentStudent != null && currentStudent.StudentId != studentId &&
                    userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                var (quizAttempts, totalCount) = await _unitOfWork.QuizAttempt.GetQuizAttemptsAsync(
                    pageNumber, pageSize, "studentid", studentId.ToString(), "starttime_desc", false, "Quiz");

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

        public async Task<ResponseDto> DeleteQuizAttempt(ClaimsPrincipal user, Guid quizAttemptId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var deleteQuizAttempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.QuizAttemptId == quizAttemptId &&
                qa.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (deleteQuizAttempt is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Verify ownership or admin rights
            var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            if (student != null && deleteQuizAttempt.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);

            deleteQuizAttempt.Status = StaticOperationStatus.BaseEntity.Deleted;
            deleteQuizAttempt.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteQuizAttempt.UpdatedBy = user.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.QuizAttempt.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok)
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
