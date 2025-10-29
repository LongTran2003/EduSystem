using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Question;

namespace EduSystem.Services.IServices;

public interface IQuestionService
{
    Task<ResponseDto> CreateQuestion(ClaimsPrincipal user, CreateQuestionDto createQuestionDto);
    Task<ResponseDto> UpdateQuestion(ClaimsPrincipal user, UpdateQuestionDto updateQuestionDto);
    Task<ResponseDto> GetAllQuestions(
        ClaimsPrincipal user,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null);
    Task<ResponseDto> GetQuestionById(ClaimsPrincipal user, Guid questionId);
    Task<ResponseDto> DeleteQuestion(ClaimsPrincipal user, Guid questionId);
}