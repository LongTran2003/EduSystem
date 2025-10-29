using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<(List<Question> questions, int totalQuestions)> GetQuestionsAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        );
        void Update(Question target, Question source);
    }
}
