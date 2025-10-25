using EduSystem.Models.DTO;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface ITeacherService
    {
        Task<ResponseDto> GetTeacherDetailsById(ClaimsPrincipal user, Guid teacherId);
        Task<ResponseDto> GetAllTeachers
            (
            ClaimsPrincipal User,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            );
        Task<ResponseDto> GetTeacherInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
    }
}
