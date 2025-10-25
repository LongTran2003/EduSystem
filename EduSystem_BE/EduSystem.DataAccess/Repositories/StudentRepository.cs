using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Models.Enums;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Student> Students, int TotalCount)> GetStudentsAsync
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
            var query = _context.Students.AsQueryable();

            // Filter by status for non-admin users
            if (!isAdmin)
            {
                query = query.Where(s => s.Status == StudentStatus.Active);
            }

            // Apply filtering
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "fullname" => query.Where(s => s.ApplicationUser.FullName.Contains(filterQuery)),
                    "email" => query.Where(s => s.ApplicationUser.Email.Contains(filterQuery)),
                    "phonenumber" => query.Where(s => s.ApplicationUser.PhoneNumber.Contains(filterQuery)),
                    "studentcode" => query.Where(s => s.StudentCode.Contains(filterQuery)),
                    "class" => query.Where(s => s.Class != null && s.Class.Contains(filterQuery)),
                    "grade" => query.Where(s => s.Grade != null && s.Grade.Contains(filterQuery)),
                    "school" => query.Where(s => s.School != null && s.School.Contains(filterQuery)),
                    "status" => Enum.TryParse<StudentStatus>(filterQuery, true, out var status)
                        ? query.Where(s => s.Status == status)
                        : query,
                    _ => query
                };
            }

            // Include properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            // Get total count before pagination
            int totalStudents = await query.CountAsync();

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "fullname" => query.OrderBy(s => s.ApplicationUser.FullName),
                "fullname_desc" => query.OrderByDescending(s => s.ApplicationUser.FullName),
                "email" => query.OrderBy(s => s.ApplicationUser.Email),
                "email_desc" => query.OrderByDescending(s => s.ApplicationUser.Email),
                "studentcode" => query.OrderBy(s => s.StudentCode),
                "studentcode_desc" => query.OrderByDescending(s => s.StudentCode),
                "enrollmentdate" => query.OrderBy(s => s.EnrollmentDate),
                "enrollmentdate_desc" => query.OrderByDescending(s => s.EnrollmentDate),
                "status" => query.OrderBy(s => s.Status),
                "status_desc" => query.OrderByDescending(s => s.Status),
                "class" => query.OrderBy(s => s.Class),
                "class_desc" => query.OrderByDescending(s => s.Class),
                _ => query.OrderBy(s => s.ApplicationUser.FullName)
            };

            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalStudents);
        }
    }
}
