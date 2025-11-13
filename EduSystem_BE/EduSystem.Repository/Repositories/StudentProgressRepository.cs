using EduSystem.Models.Entities;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
{
    public class StudentProgressRepository : Repository<StudentProgress>, IStudentProgressRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentProgressRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<StudentProgress?> GetProgressByStudentAndUnitAsync(Guid studentId, Guid unitId)
        {
            return await _context.StudentProgresses
                .Include(sp => sp.Student)
                .Include(sp => sp.Unit)
                .FirstOrDefaultAsync(sp => sp.StudentId == studentId && sp.UnitId == unitId);
        }

        public async Task<IEnumerable<StudentProgress>> GetProgressByStudentIdAsync(Guid studentId)
        {
            return await _context.StudentProgresses
                .Include(sp => sp.Student)
                .Include(sp => sp.Unit)
                .Where(sp => sp.StudentId == studentId)
                .OrderByDescending(sp => sp.LastAccessDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentProgress>> GetProgressByUnitIdAsync(Guid unitId)
        {
            return await _context.StudentProgresses
                .Include(sp => sp.Student)
                .Include(sp => sp.Unit)
                .Where(sp => sp.UnitId == unitId)
                .OrderBy(sp => sp.Student.ApplicationUser.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentProgress>> GetLowProgressStudentsAsync(decimal threshold)
        {
            return await _context.StudentProgresses
                .Include(sp => sp.Student)
                .Include(sp => sp.Unit)
                .Where(sp => sp.TotalLessons > 0 &&
                            (decimal)sp.CompletedLessons / sp.TotalLessons * 100 < threshold)
                .OrderBy(sp => sp.CompletedLessons)
                .ToListAsync();
        }

        public async Task<(List<StudentProgress> progresses, int totalProgresses)> GetStudentProgressesAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null)
        {
            var query = _context.StudentProgresses
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(sp => sp.Unit)
                .AsQueryable();

            // Filter by status for non-admin users
            if (!isAdmin)
            {
                query = query.Where(sp => sp.Status == StaticOperationStatus.BaseEntity.Active);
            }

            // Apply filters
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                filterOn = filterOn.Trim().ToLower();
                filterQuery = filterQuery.Trim();

                query = filterOn switch
                {
                    "studentname" => query.Where(sp => sp.Student != null && sp.Student.ApplicationUser.FullName.Contains(filterQuery)),
                    "unitname" => query.Where(sp => sp.Unit != null && sp.Unit.UnitName.Contains(filterQuery)),
                    "status" => query.Where(sp => sp.Status != null && sp.Status.Contains(filterQuery)),
                    _ => query
                };
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                sortBy = sortBy.Trim().ToLower();

                query = sortBy switch
                {
                    "studentname" => query.OrderBy(sp => sp.Student.ApplicationUser.FullName),
                    "studentname_desc" => query.OrderByDescending(sp => sp.Student.ApplicationUser.FullName),
                    "unitname" => query.OrderBy(sp => sp.Unit.UnitName),
                    "unitname_desc" => query.OrderByDescending(sp => sp.Unit.UnitName),
                    "progress" => query.OrderBy(sp => sp.TotalLessons > 0 ? (decimal)sp.CompletedLessons / sp.TotalLessons : 0),
                    "progress_desc" => query.OrderByDescending(sp => sp.TotalLessons > 0 ? (decimal)sp.CompletedLessons / sp.TotalLessons : 0),
                    "averagescore" => query.OrderBy(sp => sp.AverageScore),
                    "averagescore_desc" => query.OrderByDescending(sp => sp.AverageScore),
                    "lastaccessdate" => query.OrderBy(sp => sp.LastAccessDate),
                    "lastaccessdate_desc" => query.OrderByDescending(sp => sp.LastAccessDate),
                    "createdtime" => query.OrderBy(sp => sp.CreatedTime),
                    "createdtime_desc" => query.OrderByDescending(sp => sp.CreatedTime),
                    _ => query.OrderByDescending(sp => sp.LastAccessDate)
                };
            }
            else
            {
                // Default sorting: LastAccessDate DESC
                query = query.OrderByDescending(sp => sp.LastAccessDate);
            }

            // Include additional navigation properties if specified
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    var trimmedProperty = property.Trim();
                    // Skip already included properties
                    if (trimmedProperty != "Student" && trimmedProperty != "Unit")
                    {
                        query = query.Include(trimmedProperty);
                    }
                }
            }

            // Get total count
            var totalProgresses = await query.CountAsync();

            // Apply pagination
            var progresses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (progresses, totalProgresses);
        }

        public void Update(StudentProgress studentProgress)
        {
            _context.StudentProgresses.Update(studentProgress);
            _context.Entry(studentProgress).State = EntityState.Modified;
        }
    }
}
