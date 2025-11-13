using EduSystem.Models.Entities;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
{
    public class MatrixRepository : Repository<Matrix>, IMatrixRepository
    {
        private readonly ApplicationDBContext _context;
        public MatrixRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Matrix> matrices, int totalMatrices)> GetMatricesAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy, 
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.Matrices.Include(u => u.Teacher).
                ThenInclude(t => t.ApplicationUser).AsQueryable();

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
                    "name" => query.Where(s => s.Name.Contains(filterQuery)),
                    "englishlevel" => query.Where(s => s.EnglishLevel != null && s.EnglishLevel.Contains(filterQuery)),
                    "skillfocus" => query.Where(s => s.SkillFocus != null && s.SkillFocus.Contains(filterQuery)),
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
                    "name" => query.OrderBy(s => s.Name),
                    "name_desc" => query.OrderByDescending(s => s.Name),
                    "englishlevel" => query.OrderBy(s => s.EnglishLevel),
                    "englishlevel_desc" => query.OrderByDescending(s => s.EnglishLevel),
                    "skillfocus" => query.OrderBy(s => s.SkillFocus),
                    "skillfocus_desc" => query.OrderByDescending(s => s.SkillFocus),
                    "createdtime" => query.OrderByDescending(s => s.CreatedTime),
                    _ => query.OrderByDescending(s => s.CreatedTime)
                };
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedTime);
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

        public void Update(Matrix target, Matrix source)
        {
            _context.Set<Matrix>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
