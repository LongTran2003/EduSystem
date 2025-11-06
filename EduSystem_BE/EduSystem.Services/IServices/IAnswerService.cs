using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.Answer;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IAnswerService
    {
        Task<ResponseDto> CreateAnswer(ClaimsPrincipal user, CreateAnswerDto createDto);
        Task<ResponseDto> UpdateAnswer(ClaimsPrincipal user, UpdateAnswerDto updateDto);
        Task<ResponseDto> GetAllAnswers(
            ClaimsPrincipal user, 
            int pageNumber = 1, 
            int pageSize = 10,
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null);
        Task<ResponseDto> GetAnswerById(ClaimsPrincipal user, Guid answerId);
        Task<ResponseDto> GetAnswersByQuestionId(ClaimsPrincipal user, Guid questionId);
        Task<ResponseDto> DeleteAnswer(ClaimsPrincipal user, Guid answerId);
    }
}
