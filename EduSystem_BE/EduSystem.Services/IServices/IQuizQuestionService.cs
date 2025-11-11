using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.QuizQuestion;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IQuizQuestionService
    {
        Task<ResponseDto> AddQuestionToQuiz(ClaimsPrincipal user, CreateQuizQuestionDto createQuizQuestionDto);
        Task<ResponseDto> AddMultipleQuestionsToQuiz(ClaimsPrincipal user, AddQuestionsToQuizDto addQuestionsToQuizDto);
        Task<ResponseDto> GetQuestionsByQuizId(ClaimsPrincipal user, Guid quizId);
        Task<ResponseDto> GetQuizzesByQuestionId(ClaimsPrincipal user, Guid questionId);
        Task<ResponseDto> UpdateQuestionOrder(ClaimsPrincipal user, UpdateQuestionOrderDto updateQuestionOrderDto);
        Task<ResponseDto> RemoveQuestionFromQuiz(ClaimsPrincipal user, Guid quizId, Guid questionId);
        Task<ResponseDto> RemoveAllQuestionsFromQuiz(ClaimsPrincipal user, Guid quizId);
    }
}
