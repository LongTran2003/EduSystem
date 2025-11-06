using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.LessonContent;

namespace EduSystem.Services.IServices;

public interface ILessonContentService
{
    Task<ResponseDto> CreateLessonContent(ClaimsPrincipal user, CreateLessonContentDto createLessonContentDto);
    Task<ResponseDto> UpdateLessonContent(ClaimsPrincipal user, UpdateLessonContentDto updateLessonContentDto);
    Task<ResponseDto> GetAllLessonContents(
        ClaimsPrincipal user, 
        int pageNumber = 1, 
        int pageSize = 10,
        string? filterOn = null, 
        string? filterQuery = null, 
        string? sortBy = null);
    Task<ResponseDto> GetLessonContentById(ClaimsPrincipal user, Guid contentId);
    Task<ResponseDto> DeleteLessonContent(ClaimsPrincipal user, Guid contentId);
}