using AutoMapper;
using EduSystem.Repository.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.StudentProgresses;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class StudentProgressService : IStudentProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentProgressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateStudentProgress(ClaimsPrincipal user, CreateStudentProgressDto createStudentProgressDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var existingProgress = await _unitOfWork.StudentProgress
                    .GetProgressByStudentAndUnitAsync(createStudentProgressDto.StudentId, createStudentProgressDto.UnitId);

                if (existingProgress != null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.StudentProgress.AlreadyExist,
                        statusCode: StaticOperationStatus.StatusCode.BadRequest);

                var student = await _unitOfWork.Student.GetAsync(s => s.StudentId == createStudentProgressDto.StudentId);
                if (student == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Student.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var unit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == createStudentProgressDto.UnitId);
                if (unit == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Unit.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var studentProgress = _mapper.Map<StudentProgress>(createStudentProgressDto);
                studentProgress.ProgressId = Guid.NewGuid();
                studentProgress.LastAccessDate = StaticOperationStatus.Timezone.Vietnam;
                studentProgress.Status = StaticOperationStatus.BaseEntity.Active;
                studentProgress.CreatedBy = user.FindFirstValue("FullName") ?? "System";
                studentProgress.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.StudentProgress.AddAsync(studentProgress);
                await _unitOfWork.SaveAsync();

                var createdProgress = await _unitOfWork.StudentProgress.GetAsync(
                    sp => sp.ProgressId == studentProgress.ProgressId,
                    includeProperties: "Student,Unit");

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<StudentProgressDto>(createdProgress));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateStudentProgress(ClaimsPrincipal user, UpdateStudentProgressDto updateStudentProgressDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var studentProgress = await _unitOfWork.StudentProgress.GetAsync(
                    sp => sp.ProgressId == updateStudentProgressDto.ProgressId,
                    includeProperties: "Student");

                if (studentProgress == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.StudentProgress.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && studentProgress.StudentId != student.StudentId &&
                    userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                _mapper.Map(updateStudentProgressDto, studentProgress);
                studentProgress.LastAccessDate = StaticOperationStatus.Timezone.Vietnam;
                studentProgress.UpdatedBy = user.FindFirstValue("FullName") ?? "System";
                studentProgress.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                _unitOfWork.StudentProgress.Update(studentProgress);
                await _unitOfWork.SaveAsync();

                var updatedProgress = await _unitOfWork.StudentProgress.GetAsync(
                    sp => sp.ProgressId == studentProgress.ProgressId,
                    includeProperties: "Student,Unit");

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<StudentProgressDto>(updatedProgress));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> CompleteLessonProgress(ClaimsPrincipal user, UpdateProgressDto updateProgressDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                if (student == null || student.StudentId != updateProgressDto.StudentId)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                var lesson = await _unitOfWork.Lesson.GetAsync(l => l.LessonId == updateProgressDto.CompletedLessonId);
                if (lesson == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Lesson.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var studentProgress = await _unitOfWork.StudentProgress
                    .GetProgressByStudentAndUnitAsync(updateProgressDto.StudentId, updateProgressDto.UnitId);

                if (studentProgress == null)
                {
                    var unit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == updateProgressDto.UnitId);
                    if (unit == null)
                        return ErrorResponse.Build(
                            message: StaticResponseMessage.Unit.NotFound,
                            statusCode: StaticOperationStatus.StatusCode.NotFound);

                    var allLessons = await _unitOfWork.Lesson.GetAllAsync(
                        filter: l => l.UnitId == updateProgressDto.UnitId &&
                            l.Status != StaticOperationStatus.BaseEntity.Deleted);
                    var totalLessons = allLessons.Count();

                    studentProgress = new StudentProgress
                    {
                        ProgressId = Guid.NewGuid(),
                        StudentId = updateProgressDto.StudentId,
                        UnitId = updateProgressDto.UnitId,
                        CompletedLessons = 1,
                        TotalLessons = totalLessons,
                        AverageScore = updateProgressDto.LessonScore ?? 0,
                        TotalTimeSpent = updateProgressDto.TimeSpent ?? 0,
                        LastAccessDate = StaticOperationStatus.Timezone.Vietnam,
                        Status = StaticOperationStatus.BaseEntity.Active,
                        CreatedBy = user.FindFirstValue("FullName") ?? "System",
                        CreatedTime = StaticOperationStatus.Timezone.Vietnam
                    };

                    await _unitOfWork.StudentProgress.AddAsync(studentProgress);
                }
                else
                {
                    studentProgress.CompletedLessons += 1;
                    if (studentProgress.CompletedLessons > studentProgress.TotalLessons)
                        studentProgress.CompletedLessons = studentProgress.TotalLessons;

                    if (updateProgressDto.LessonScore.HasValue)
                    {
                        var currentTotal = studentProgress.AverageScore * (studentProgress.CompletedLessons - 1);
                        studentProgress.AverageScore = (currentTotal + updateProgressDto.LessonScore.Value) / studentProgress.CompletedLessons;
                    }

                    if (updateProgressDto.TimeSpent.HasValue)
                        studentProgress.TotalTimeSpent += updateProgressDto.TimeSpent.Value;

                    studentProgress.LastAccessDate = StaticOperationStatus.Timezone.Vietnam;
                    studentProgress.UpdatedBy = user.FindFirstValue("FullName") ?? "System";
                    studentProgress.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                    _unitOfWork.StudentProgress.Update(studentProgress);
                }

                await _unitOfWork.SaveAsync();

                var updatedProgress = await _unitOfWork.StudentProgress.GetAsync(
                    sp => sp.ProgressId == studentProgress.ProgressId,
                    includeProperties: "Student,Unit");

                return SuccessResponse.Build(
                    message: "Lesson completed successfully",
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<StudentProgressDto>(updatedProgress));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetStudentProgressById(ClaimsPrincipal user, Guid progressId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var studentProgress = await _unitOfWork.StudentProgress.GetAsync(
                    sp => sp.ProgressId == progressId,
                    includeProperties: "Student,Unit");

                if (studentProgress == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.StudentProgress.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && studentProgress.StudentId != student.StudentId &&
                    userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<StudentProgressDto>(studentProgress));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetProgressByStudentId(ClaimsPrincipal user, Guid studentId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && student.StudentId != studentId &&
                    userRole != StaticUserRoles.Admin && userRole != StaticUserRoles.Teacher)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                var progresses = await _unitOfWork.StudentProgress.GetProgressByStudentIdAsync(studentId);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<IEnumerable<StudentProgressDto>>(progresses));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetProgressByUnitId(ClaimsPrincipal user, Guid unitId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var progresses = await _unitOfWork.StudentProgress.GetProgressByUnitIdAsync(unitId);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<IEnumerable<StudentProgressDto>>(progresses));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAllStudentProgress(
            ClaimsPrincipal user, 
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                // Check if user is Admin or Teacher
                var userRole = user.FindFirstValue(ClaimTypes.Role);
                var isAdmin = userRole == StaticUserRoles.Admin;

                // Get progresses with pagination from repository
                var (progresses, totalProgresses) = await _unitOfWork.StudentProgress.GetStudentProgressesAsync(
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    filterOn: filterOn,
                    filterQuery: filterQuery,
                    sortBy: sortBy,
                    isAdmin: isAdmin,
                    includeProperties: "Student,Unit");

                // Map to DTOs
                var progressDtos = _mapper.Map<IEnumerable<StudentProgressDto>>(progresses);

                // Create result object
                var result = new
                {
                    Data = progressDtos,
                    TotalCount = totalProgresses,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalProgresses / (double)pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> DeleteStudentProgress(ClaimsPrincipal user, Guid progressId)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var studentProgress = await _unitOfWork.StudentProgress.GetAsync(sp => sp.ProgressId == progressId);
                if (studentProgress == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.StudentProgress.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var student = await _unitOfWork.Student.GetAsync(s => s.UserId == userId);
                var userRole = user.FindFirstValue(ClaimTypes.Role);

                if (student != null && studentProgress.StudentId != student.StudentId && userRole != StaticUserRoles.Admin)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Forbidden);

                _unitOfWork.StudentProgress.Remove(studentProgress);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.StudentProgress.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.StudentProgress.NotDeleted + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

        }
    }
}
