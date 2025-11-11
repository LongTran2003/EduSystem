using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.StudentProgresses;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IStudentProgressService
    {
        Task<ResponseDto> CreateStudentProgress(ClaimsPrincipal user, CreateStudentProgressDto createStudentProgressDto);
        Task<ResponseDto> UpdateStudentProgress(ClaimsPrincipal user, UpdateStudentProgressDto updateStudentProgressDto);
        Task<ResponseDto> CompleteLessonProgress(ClaimsPrincipal user, UpdateProgressDto updateProgressDto);
        Task<ResponseDto> GetStudentProgressById(ClaimsPrincipal user, Guid progressId);
        Task<ResponseDto> GetProgressByStudentId(ClaimsPrincipal user, Guid studentId);
        Task<ResponseDto> GetProgressByUnitId(ClaimsPrincipal user, Guid unitId);
        Task<ResponseDto> GetAllStudentProgress(ClaimsPrincipal user, int pageNumber, int pageSize, string? filterOn, string? filterQuery, string? sortBy);
        Task<ResponseDto> DeleteStudentProgress(ClaimsPrincipal user, Guid progressId);
    }
}
