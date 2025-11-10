using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.QuizAttempt;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IQuizAttemptService
    {
        Task<ResponseDto> CreateQuizAttempt(ClaimsPrincipal user, CreateQuizAttemptDto createQuizAttemptDto);
        Task<ResponseDto> UpdateQuizAttempt(ClaimsPrincipal user, UpdateQuizAttemptDto updateQuizAttemptDto);
        Task<ResponseDto> GetQuizAttemptById(ClaimsPrincipal user, Guid quizAttemptId);
        Task<ResponseDto> GetAllQuizAttempts(
            ClaimsPrincipal user, 
            int pageNumber = 1, 
            int pageSize = 10,
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null);
        Task<ResponseDto> GetQuizAttemptsByQuizId(
            ClaimsPrincipal user, 
            Guid quizId, 
            int pageNumber = 1, 
            int pageSize = 10);
        Task<ResponseDto> GetQuizAttemptsByStudentId(
            ClaimsPrincipal user, 
            Guid studentId, 
            int pageNumber = 1, 
            int pageSize = 10);
        Task<ResponseDto> DeleteQuizAttempt(ClaimsPrincipal user, Guid quizAttemptId);

    }
}
