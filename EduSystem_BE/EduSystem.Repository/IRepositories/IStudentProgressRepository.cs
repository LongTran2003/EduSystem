using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IStudentProgressRepository : IRepository<StudentProgress>
    {
        Task<StudentProgress?> GetProgressByStudentAndUnitAsync(Guid studentId, Guid unitId);
        Task<IEnumerable<StudentProgress>> GetProgressByStudentIdAsync(Guid studentId);
        Task<IEnumerable<StudentProgress>> GetProgressByUnitIdAsync(Guid unitId);
        Task<IEnumerable<StudentProgress>> GetLowProgressStudentsAsync(decimal threshold);
        Task<(List<StudentProgress> progresses, int totalProgresses)> GetStudentProgressesAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null);
        void Update(StudentProgress studentProgress);
    }
}
