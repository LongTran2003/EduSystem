using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.StudentAnswers;
using EduSystem.Models.DTOs.StudentAnswers.SubmitAnswer;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IStudentAnswerService
    {
        Task<ResponseDto> CreateStudentAnswer(ClaimsPrincipal user, CreateStudentAnswerDto createStudentAnswerDto);
        Task<ResponseDto> SubmitAnswers(ClaimsPrincipal user, SubmitAnswersDto submitAnswersDto);
        Task<ResponseDto> UpdateStudentAnswer(ClaimsPrincipal user, UpdateStudentAnswerDto updateStudentAnswerDto);
        Task<ResponseDto> GetStudentAnswerById(ClaimsPrincipal user, Guid attemptId, Guid questionId);
        Task<ResponseDto> GetAllStudentAnswers(ClaimsPrincipal user, int pageNumber = 1, int pageSize = 10,
            string? filterOn = null, string? filterQuery = null, string? sortBy = null);
        Task<ResponseDto> GetAnswersByAttemptId(ClaimsPrincipal user, Guid attemptId);
        Task<ResponseDto> GradeAnswer(ClaimsPrincipal user, UpdateStudentAnswerDto gradeDto);
        Task<ResponseDto> DeleteStudentAnswer(ClaimsPrincipal user, Guid attemptId, Guid questionId);
    }
}
