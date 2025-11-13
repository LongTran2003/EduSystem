using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface ILessonContentRepository : IRepository<LessonContent>
    {
        Task<(List<LessonContent> lessonContents, int totalLessonContents)> GetLessonContentsAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        );
        void Update(LessonContent target, LessonContent source);
        
    }
}
