using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<(List<Subject> Subjects, int TotalSubjects)> GetSubjectsAsync
            (
                int pageNumber,
                int pageSize,
                string? filterOn,
                string? filterQuery,
                string? sortBy,
                bool isAdmin = false,
                string? includeProperties = null
            );
        void Update(Subject target, Subject source);
    }
}
