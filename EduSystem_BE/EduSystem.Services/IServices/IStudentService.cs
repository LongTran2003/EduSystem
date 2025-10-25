using EduSystem.Models.DTO;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IStudentService
    {
        Task<ResponseDto> GetStudentDetailsById(ClaimsPrincipal user, Guid studentId);
        Task<ResponseDto> GetAllStudent
            (
            ClaimsPrincipal User,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            );
        Task<ResponseDto> GetStudentInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
    }
}
