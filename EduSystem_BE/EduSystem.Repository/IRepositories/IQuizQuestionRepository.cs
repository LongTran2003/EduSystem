using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IQuizQuestionRepository : IRepository<QuizQuestion>
    {
        Task<QuizQuestion?> GetQuizQuestionAsync(Guid quizId, Guid questionId);
        Task<IEnumerable<QuizQuestion>> GetQuestionsByQuizIdAsync(Guid quizId);
        Task<IEnumerable<QuizQuestion>> GetQuizzesByQuestionIdAsync(Guid questionId);
        Task<bool> IsQuestionInQuizAsync(Guid quizId, Guid questionId);
        Task<int> GetMaxQuestionOrderAsync(Guid quizId);
        Task RemoveQuestionsByQuizIdAsync(Guid quizId);

        void Update(QuizQuestion quizQuestion);
    }
}
