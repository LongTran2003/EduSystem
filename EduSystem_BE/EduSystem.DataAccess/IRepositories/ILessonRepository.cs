using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<(List<Lesson> Lessons, int TotalLessons)> GetLessonsAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        );
        void Update(Lesson target, Lesson source);
        
    }
}
