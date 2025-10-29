using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Quiz;

namespace EduSystem.Services.IServices;

public interface IQuizService
{
    Task<ResponseDto> CreateQuiz(ClaimsPrincipal user, CreateQuizDto createQuizDto);
    Task<ResponseDto> UpdateQuiz(ClaimsPrincipal user, UpdateQuizDto updateQuizDto);
    Task<ResponseDto> GetAllQuizzes
    (
        ClaimsPrincipal user,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
    Task<ResponseDto> GetQuizById(ClaimsPrincipal user, Guid quizId);
    Task<ResponseDto> DeleteQuiz(ClaimsPrincipal user, Guid quizId);
}