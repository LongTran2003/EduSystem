using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Models.Enums;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDBContext _context;
        public SubjectRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Subject> subjects, int totalSubjects)> GetSubjectsAsync
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
            var query = _context.Subjects.AsQueryable();

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
                    "subjectname" => query.Where(s => s.SubjectName.Contains(filterQuery)),
                    "subjectcode" => query.Where(s => s.SubjectCode != null && s.SubjectCode.Contains(filterQuery)),
                    "gradelevel" => query.Where(s => s.GradeLevel != null && s.GradeLevel.Contains(filterQuery)),
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
                    "subjectname" => query.OrderBy(s => s.SubjectName),
                    "subjectnamedesc" => query.OrderByDescending(s => s.SubjectName),
                    "subjectcode" => query.OrderBy(s => s.SubjectCode),
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
            var totalSubjects = await query.CountAsync();

            // Apply pagination
            var subjects = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (subjects, totalSubjects);
        }
        

        public void Update(Subject target, Subject source)
        {
            _context.Set<Subject>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
