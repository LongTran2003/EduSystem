using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<(List<Quiz> quizzes, int totalQuizzes)> GetQuizzesAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        );
        void Update(Quiz target, Quiz source);
        
        
    }
}
