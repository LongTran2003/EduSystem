using EduSystem.Models.DTO;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface ITeacherService
    {
        Task<ResponseDto> GetTeacherDetailsById(ClaimsPrincipal user, Guid teacherId);
        Task<ResponseDto> GetAllTeachers();
        Task<ResponseDto> GetTeacherInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
    }
}
