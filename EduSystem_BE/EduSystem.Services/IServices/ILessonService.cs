using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Lesson;

namespace EduSystem.Services.IServices;

public interface ILessonService
{
    Task<ResponseDto> CreateLesson(ClaimsPrincipal user, CreateLessonDto createLessonDto);
    Task<ResponseDto> UpdateLesson(ClaimsPrincipal user, UpdateLessonDto updateLessonDto);
    Task<ResponseDto> GetAllLessons
    (
        ClaimsPrincipal user,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
    Task<ResponseDto> GetLessonById(ClaimsPrincipal user, Guid lessonId);
    Task<ResponseDto> DeleteLesson(ClaimsPrincipal user, Guid lessonId);
}