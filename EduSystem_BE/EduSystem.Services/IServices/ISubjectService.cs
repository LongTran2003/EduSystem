using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Subject;

namespace EduSystem.Services.IServices;

public interface ISubjectService
{
    Task<ResponseDto> CreateSubject(ClaimsPrincipal user, CreateSubjectDto createSubjectDto);
    Task<ResponseDto> UpdateSubject(ClaimsPrincipal user, UpdateSubjectDto updateSubjectDto);
    Task<ResponseDto> GetAllSubjects
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
    Task<ResponseDto> GetSubjectById(ClaimsPrincipal user, Guid subjectId);
    Task<ResponseDto> DeleteSubject(ClaimsPrincipal user, Guid subjectId);
}