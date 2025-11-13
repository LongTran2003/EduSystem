using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<(List<Teacher> Teachers, int TotalTeachers)> GetTeachersAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null);

        void Update(Teacher teacher);
        Task<string> GetNextTeacherCodeAsync();
        Task<string> CalculateTeachingExperienceAsync(int initialYears);
    }
}
