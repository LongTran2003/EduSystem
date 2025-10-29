using EduSystem.Models.DTO;
using System.Security.Claims;
using EduSystem.Models.DTO.Student;

namespace EduSystem.Services.IServices
{
    public interface IStudentService
    {
        Task<ResponseDto> GetStudentDetailsById(ClaimsPrincipal user, Guid studentId);
        Task<ResponseDto> GetAllStudent
            (
            ClaimsPrincipal user,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            );
        Task<ResponseDto> GetStudentInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
        Task<ResponseDto> UpdateStudentStatus(ClaimsPrincipal user, UpdateStudentStatusDto updateStudentStatusDto);
    }
}
