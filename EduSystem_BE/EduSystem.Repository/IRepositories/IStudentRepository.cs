using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<(List<Student> Students, int TotalCount)> GetStudentsAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        );

        Task<string> GetNextStudentCodeAsync();
    }
}
