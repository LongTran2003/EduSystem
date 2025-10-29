using System.Security.Claims;
using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Subject;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class SubjectService : ISubjectService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateSubject(ClaimsPrincipal user, CreateSubjectDto createSubjectDto)
    {
        try
        {
            var subject = _mapper.Map<CreateSubjectDto, Subject>(createSubjectDto);
            subject.CreatedBy = user.FindFirstValue("Fullname");
            subject.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

            await _unitOfWork.Subject.AddAsync(subject);
            await _unitOfWork.SaveAsync();

            return SuccessResponse.Build(
                message: StaticResponseMessage.Subject.Created,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: subject);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> UpdateSubject(ClaimsPrincipal user, UpdateSubjectDto updateSubjectDto)
    {
        var subject = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == updateSubjectDto.SubjectId);
        if (subject == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotFound,
                statusCode: StaticOperationStatus.StatusCode.Ok);
        }
        
        var updateSubject = _mapper.Map<UpdateSubjectDto, Subject>(updateSubjectDto);
        subject.UpdatedBy = user.FindFirstValue("Fullname");
        subject.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        subject.Status = updateSubject.Status;
        
        // Update Subject
        _unitOfWork.Subject.Update(subject, updateSubject);

        return (!await SaveChangesAsync()) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Subject.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: updateSubject);

    }

    public async Task<ResponseDto> GetAllSubjects
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

            var (subjects, totalSubjects) = await _unitOfWork.Subject.GetSubjectsAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (subjects == null || !subjects.Any() || totalSubjects == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<Subject>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Subject.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }
            
            var subjectsDto = _mapper.Map<IEnumerable<Subject>>(subjects);

            var result = new
            {
                Data = subjectsDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalSubjects,
                TotalPages = (int)Math.Ceiling((double)totalSubjects / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalSubjects / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.Subject.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> GetSubjectById(ClaimsPrincipal user, Guid subjectId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        var getSubjectById = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == subjectId
                            && s.Status != StaticOperationStatus.BaseEntity.Deleted);
        return (getSubjectById is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound) 
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Subject.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: getSubjectById);
    }

    public async Task<ResponseDto> DeleteSubject(ClaimsPrincipal user, Guid subjectId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var deleteSubject = await _unitOfWork.Subject.GetAsync(s => s.SubjectId == subjectId
                            &&  s.Status != StaticOperationStatus.BaseEntity.Deleted);
        if (deleteSubject is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        deleteSubject.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteSubject.UpdatedBy = user.FindFirstValue("Fullname");
        deleteSubject.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Subject.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: deleteSubject)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Subject.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}