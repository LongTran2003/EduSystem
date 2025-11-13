using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        private readonly ApplicationDBContext _context;
        public LessonRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<(List<Lesson> Lessons, int TotalLessons)> GetLessonsAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null
        )
        {
            var query = _context.Lessons.Include(l => l.Unit).AsQueryable();

            // Filter by status for non-admin users
            if (!isAdmin)
            {
                query = query.Where(s => s.Status == StaticOperationStatus.BaseEntity.Active);
            }

            // Apply filters
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                filterOn = filterOn.Trim().ToLower();
                filterQuery = filterQuery.Trim();

                query = filterOn switch
                {
                    "lessonname" => query.Where(s => s.LessonName.Contains(filterQuery)),
                    "skill" => query.Where(s => s.Skill != null && s.Skill.Contains(filterQuery)),
                    "content" => query.Where(s => s.Content != null && s.Content.Contains(filterQuery)),
                    "status" => query.Where(s => s.Status == StaticOperationStatus.BaseEntity.Active),
                    _ => query
                };
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                sortBy = sortBy.Trim().ToLower();

                query = sortBy switch
                {
                    "lessonname" => query.OrderBy(s => s.LessonName),
                    "lessonname_desc" => query.OrderByDescending(s => s.LessonName),
                    "skill" => query.OrderBy(s => s.Skill),
                    "content" => query.OrderBy(s => s.Content),
                    "orderindex" => query.OrderBy(s => s.OrderIndex),
                    "orderindex_desc" => query.OrderByDescending(s => s.OrderIndex),
                    "createdtime" => query.OrderByDescending(s => s.CreatedTime),
                    _ => query.OrderByDescending(s => s.CreatedTime)
                };
            }
            else
            {
                query = query.OrderByDescending(s => s.OrderIndex).ThenByDescending(s => s.CreatedTime); ;
            }

            // Include navigation properties if specified
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            // Get total count
            var totalLessons = await query.CountAsync();

            // Apply pagination
            var lessons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (lessons, totalLessons);
        }
        
        public void Update(Lesson target, Lesson source)
        {
            _context.Set<Lesson>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
