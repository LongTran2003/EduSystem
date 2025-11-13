using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<(List<Answer> answers, int totalAnswers)> GetAnswersAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null);

        void Update(Answer target, Answer source);
        void Update(Answer answer);

        Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid questionId);
    }
        
}
