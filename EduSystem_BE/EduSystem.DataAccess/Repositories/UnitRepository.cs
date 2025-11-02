using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        private readonly ApplicationDBContext _context;

        public UnitRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Unit> units, int totalUnits)> GetUnitsAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy,
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.Units.Include(u => u.Teacher).ThenInclude(t => t.ApplicationUser).AsQueryable();

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
                    "unitname" => query.Where(s => s.UnitName.Contains(filterQuery)),
                    "englishlevel" => query.Where(s => s.EnglishLevel != null && s.EnglishLevel.Contains(filterQuery)),
                    "learningobjects" => query.Where(s => s.LearningObjectives != null && s.LearningObjectives.Contains(filterQuery)),
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
                    "unitname" => query.OrderBy(s => s.UnitName),
                    "unitname_desc" => query.OrderByDescending(s => s.UnitName),
                    "englishlevel" => query.OrderBy(s => s.EnglishLevel),
                    "learningobjects" => query.OrderBy(s => s.LearningObjectives),
                    "orderindex" => query.OrderBy(s => s.OrderIndex),
                    "orderindex_desc" => query.OrderByDescending(s => s.OrderIndex),
                    "createdtime" => query.OrderByDescending(s => s.CreatedTime),
                    _ => query.OrderByDescending(s => s.CreatedTime)
                };
            }
            else
            {
                query = query.OrderByDescending(s => s.OrderIndex).ThenByDescending(s => s.CreatedTime);
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
            var totalUnits = await query.CountAsync();

            // Apply pagination
            var units = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (units, totalUnits);
        }

        public void Update(Unit target, Unit source)
        {
            _context.Set<Unit>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
