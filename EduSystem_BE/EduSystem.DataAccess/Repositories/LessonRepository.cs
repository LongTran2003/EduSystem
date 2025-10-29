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
            var query = _context.Lessons.AsQueryable();

            // Filter by status for non-admin users
            if (!isAdmin)
            {
                query = query.Where(s => s.Status == StaticOperationStatus.BaseEntity.Active);
            }

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                filterOn = filterOn.Trim().ToLower();
                filterQuery = filterQuery.Trim();

                query = filterOn switch
                {
                    "title" => query.Where(s => s.Title.Contains(filterQuery)),
                    "gradelevel" => query.Where(s => s.GradeLevel != null && s.GradeLevel.Contains(filterQuery)),
                    "lessontype" => query.Where(s => s.LessonType != null && s.LessonType.Contains(filterQuery)),
                    "difficultylevel" => query.Where(s =>
                        s.DifficultyLevel != null && s.DifficultyLevel.Contains(filterQuery)),
                    "status" => query.Where(s => s.Status == StaticOperationStatus.BaseEntity.Active),
                    _ => query
                };
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = sortBy.Trim().ToLower();

                query = sortBy switch
                {
                    "title" => query.OrderBy(s => s.Title),
                    "titledesc" => query.OrderByDescending(s => s.Title),
                    "lessontype" => query.OrderBy(s => s.LessonType),
                    "gradelevel" => query.OrderBy(s => s.GradeLevel),
                    "createdtime" => query.OrderByDescending(s => s.CreatedTime),
                    _ => query.OrderByDescending(s => s.CreatedTime)
                };
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedTime);
            }

            // Include navigation properties if specified
            if (!string.IsNullOrWhiteSpace(includeProperties))
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
