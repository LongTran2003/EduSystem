using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class LessonContentRepository : Repository<LessonContent>, ILessonContentRepository
    {
        private readonly ApplicationDBContext _context;
        public LessonContentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<LessonContent> lessonContents, int totalLessonContents)> GetLessonContentsAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy,
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.LessonContents.Include(lc => lc.Lesson).AsQueryable();

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
                    "resourcetpye" => query.Where(s => s.ResourceType != null && s.ResourceType.Contains(filterQuery)),
                    "resourceurl" => query.Where(s =>  s.ResourceUrl.Contains(filterQuery)),
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
                    "resourcetpye" => query.OrderBy(s => s.ResourceType),
                    "resourcetpye_desc" => query.OrderByDescending(s => s.ResourceType),
                    "resourceurl" => query.OrderBy(s => s.ResourceUrl),
                    "resourceurl_desc" => query.OrderByDescending(s => s.ResourceUrl),
                    "createdtime" => query.OrderByDescending(s => s.CreatedTime),
                    _ => query.OrderByDescending(s => s.CreatedTime)
                };
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedTime).ThenByDescending(s => s.CreatedTime);
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

            var totalContents = await query.CountAsync();

            var contents = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (contents, totalContents);
        }

        public void Update(LessonContent target, LessonContent source)
        {
            _context.Set<LessonContent>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
