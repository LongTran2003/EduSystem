using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IStudentAnswerRepository : IRepository<StudentAnswer>
    {
        Task<(IEnumerable<StudentAnswer> items, int totalCount)> GetStudentAnswersAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin,
            string? includeProperties = null);

        Task<IEnumerable<StudentAnswer>> GetAnswersByAttemptIdAsync(Guid attemptId);

        Task<StudentAnswer?> GetAnswerByAttemptAndQuestionAsync(Guid attemptId, Guid questionId);

        void Update (StudentAnswer target, StudentAnswer source);
    }
}
