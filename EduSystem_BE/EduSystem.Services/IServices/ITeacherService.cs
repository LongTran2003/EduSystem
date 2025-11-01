using EduSystem.Models.DTO;
using System.Security.Claims;
using EduSystem.Models.DTOs.Teacher;

namespace EduSystem.Services.IServices
{
    public interface ITeacherService
    {
        Task<ResponseDto> GetTeacherDetailsById(ClaimsPrincipal user, Guid teacherId);
        Task<ResponseDto> GetAllTeachers
            (
            ClaimsPrincipal user,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            );
        Task<ResponseDto> GetTeacherInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber);
        Task<ResponseDto> UpdateTeacherStatus(ClaimsPrincipal user, UpdateTeacherStatusDto updateTeacherStatusDto);
    }
}
