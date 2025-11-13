using System.Security.Claims;
using AutoMapper;
using EduSystem.Repository.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.LessonContent;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class LessonContentService : ILessonContentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LessonContentService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ResponseDto> CreateLessonContent(ClaimsPrincipal user, CreateLessonContentDto createLessonContentDto)
    {
        try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
                if (teacher is null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Teacher.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var lesson = await _unitOfWork.Lesson.GetAsync(l => l.LessonId == createLessonContentDto.LessonId 
                    && l.Status != StaticOperationStatus.BaseEntity.Deleted);
                if (lesson == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Lesson.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var content = _mapper.Map<LessonContent>(createLessonContentDto);
                content.Status = StaticOperationStatus.BaseEntity.Active;
                content.CreatedBy = user.FindFirstValue("Fullname");
                content.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.LessonContent.AddAsync(content);
                await _unitOfWork.SaveAsync();

                var resultDto = _mapper.Map<LessonContentDto>(content);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.LessonContent.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: resultDto);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.LessonContent.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
    }

    public async Task<ResponseDto> UpdateLessonContent(ClaimsPrincipal user, UpdateLessonContentDto updateLessonContentDto)
    {
        try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var content = await _unitOfWork.LessonContent.GetAsync(
                    lc => lc.LessonContentId == updateLessonContentDto.LessonContentId 
                    && lc.Status != StaticOperationStatus.BaseEntity.Deleted);
                
                if (content == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.LessonContent.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var lesson = await _unitOfWork.Lesson.GetAsync(l => l.LessonId == updateLessonContentDto.LessonId 
                    && l.Status != StaticOperationStatus.BaseEntity.Deleted);
                if (lesson == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Lesson.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                var updateContent = _mapper.Map<LessonContent>(updateLessonContentDto);
                updateContent.Status = content.Status;
                updateContent.CreatedBy = content.CreatedBy;
                updateContent.CreatedTime = content.CreatedTime;
                updateContent.UpdatedBy = user.FindFirstValue("Fullname");
                updateContent.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                _unitOfWork.LessonContent.Update(content, updateContent);
                
                var success = await SaveChangesAsync();
                if (!success)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.LessonContent.NotUpdated,
                        statusCode: StaticOperationStatus.StatusCode.InternalServerError);

                var resultDto = _mapper.Map<LessonContentDto>(updateContent);
                return SuccessResponse.Build(
                    message: StaticResponseMessage.LessonContent.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.LessonContent.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
    }

    public async Task<ResponseDto> GetAllLessonContents(
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

            var (contents, totalContents) = await _unitOfWork.LessonContent.GetLessonContentsAsync(
                pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (contents == null || !contents.Any() || totalContents == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<LessonContent>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.LessonContent.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }

            var contentsDto = _mapper.Map<List<LessonContentDto>>(contents);
            var result = new 
            {
                Data = contentsDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalContents,
                TotalPages = (int)Math.Ceiling((double)totalContents / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalContents / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.LessonContent.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.LessonContent.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> GetLessonContentById(ClaimsPrincipal user, Guid contentId)
    {
        try
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var content = await _unitOfWork.LessonContent.GetAsync(
                lc => lc.LessonContentId == contentId 
                      && lc.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (content == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.LessonContent.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            var resultDto = _mapper.Map<LessonContentDto>(content);
            return SuccessResponse.Build(
                message: StaticResponseMessage.LessonContent.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.LessonContent.NotFound + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> DeleteLessonContent(ClaimsPrincipal user, Guid contentId)
    {
        try
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);

            var content = await _unitOfWork.LessonContent.GetAsync(
                lc => lc.LessonContentId == contentId 
                      && lc.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (content == null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.LessonContent.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            content.Status = StaticOperationStatus.BaseEntity.Deleted;
            content.UpdatedBy = user.FindFirstValue("Fullname");
            content.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            var success = await SaveChangesAsync();
            if (!success)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.LessonContent.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);

            var resultDto = _mapper.Map<LessonContentDto>(content);
            return SuccessResponse.Build(
                message: StaticResponseMessage.LessonContent.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.LessonContent.NotDeleted + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}