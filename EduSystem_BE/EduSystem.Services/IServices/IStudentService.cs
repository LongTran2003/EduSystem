using EduSystem.Models.DTO;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IStudentService
    {
        Task<ResponseDto> GetStudentDetailsById(ClaimsPrincipal user, Guid studentId);
        Task<ResponseDto> GetAllStudent();
        Task<ResponseDto> GetStudentInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
    }
}
