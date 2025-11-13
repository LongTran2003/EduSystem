using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IQuizAttemptRepository : IRepository<QuizAttempt>
    {
        Task<(List<QuizAttempt> quizAttempts, int totalCount)> GetQuizAttemptsAsync(
            int pageNumber,
            int pageSize,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAdmin = false,
            string? includeProperties = null);

        void Update (QuizAttempt target, QuizAttempt source);
    }
}
