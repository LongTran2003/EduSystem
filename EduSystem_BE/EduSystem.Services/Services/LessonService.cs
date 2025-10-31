using System.Security.Claims;
using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Lesson;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class LessonService : ILessonService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public LessonService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto> CreateLesson(ClaimsPrincipal user, CreateLessonDto createLessonDto)
    {
        try
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: "Unauthorized", 
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
            if (teacher is null)
                return ErrorResponse.Build(
                    message: "Teacher not found for current user", 
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var subject = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == createLessonDto.SubjectId);
            if (subject is null)
                return ErrorResponse.Build(
                    message: "Subject not found", 
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);

            var lesson = _mapper.Map<CreateLessonDto, Lesson>(createLessonDto);
            lesson.CreatedBy = user.FindFirstValue("Fullname");
            lesson.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.Lesson.AddAsync(lesson);
            await _unitOfWork.SaveAsync();

            return SuccessResponse.Build(
                message: StaticResponseMessage.Lesson.Created,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: lesson);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> UpdateLesson(ClaimsPrincipal user, UpdateLessonDto updateLessonDto)
    {
        var lesson = await _unitOfWork.Lesson.GetAsync(s => s.LessonId == updateLessonDto.LessonId);
        if (lesson == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotFound,
                statusCode: StaticOperationStatus.StatusCode.Ok);
        }
        
        var updateLesson = _mapper.Map<UpdateLessonDto, Lesson>(updateLessonDto);
        lesson.UpdatedBy = user.FindFirstValue("Fullname");
        lesson.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        lesson.Status = updateLesson.Status;
        
        // Update Subject
        _unitOfWork.Lesson.Update(lesson, updateLesson);

        return (!await SaveChangesAsync()) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Lesson.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: updateLesson);
    }

    public async Task<ResponseDto> GetAllLessons
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

            var (lessons, totalLessons) = await _unitOfWork.Lesson.GetLessonsAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (lessons == null || !lessons.Any() || totalLessons == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<Lesson>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Lesson.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }
            
            var lessonsDto = _mapper.Map<IEnumerable<Lesson>>(lessons);

            var result = new
            {
                Data = lessonsDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalLessons,
                TotalPages = (int)Math.Ceiling((double)totalLessons / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalLessons / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.Lesson.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> GetLessonById(ClaimsPrincipal user, Guid lessonId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        var getLessonById = await _unitOfWork.Lesson.GetAsync(s => s.LessonId == lessonId 
                                                                    && s.Status != StaticOperationStatus.BaseEntity.Deleted);
        return (getLessonById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Lesson.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: getLessonById);
    }

    public async Task<ResponseDto> DeleteLesson(ClaimsPrincipal user, Guid lessonId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var deleteLesson = await _unitOfWork.Lesson.GetAsync(s => s.LessonId == lessonId
                                                                    &&  s.Status != StaticOperationStatus.BaseEntity.Deleted);
        if (deleteLesson is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        deleteLesson.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteLesson.UpdatedBy = user.FindFirstValue("Fullname");
        deleteLesson.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Lesson.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: deleteLesson)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Lesson.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}