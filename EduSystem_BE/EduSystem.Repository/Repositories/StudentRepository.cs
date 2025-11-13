using EduSystem.Models.Entities;
using EduSystem.Models.Enums;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
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
                    "email" => query.Where(s => s.ApplicationUser.Email != null && s.ApplicationUser.Email.Contains(filterQuery)),
                    "phonenumber" => query.Where(s => s.ApplicationUser.PhoneNumber != null && s.ApplicationUser.PhoneNumber.Contains(filterQuery)),
                    "studentcode" => query.Where(s => s.StudentCode.Contains(filterQuery)),
                    "grade" => query.Where(s => s.Grade != null && s.Grade.Contains(filterQuery)),
                    "school" => query.Where(s => s.Class != null && s.Class.Contains(filterQuery)),
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
                    if (property != nameof(ApplicationUser)) // Tránh include ApplicationUser 2 lần
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
                "grade" => query.OrderBy(s => s.Grade),
                "grade_decs" => query.OrderByDescending(s => s.Grade),
                "class" => query.OrderBy(s => s.Class),
                "class_decs" => query.OrderByDescending(s => s.Class),
                "status" => query.OrderBy(s => s.Status),
                "status_desc" => query.OrderByDescending(s => s.Status),
                _ => query.OrderBy(s => s.ApplicationUser.FullName)
            };

            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalStudents);
        }

        public async Task<string> GetNextStudentCodeAsync()
        {
            // Prefix "ENG" for English Education System
            const string SYSTEM_PREFIX = "ENG";

            // Get current academic year
            int currentYear = DateTime.Now.Year;
            string yearPrefix = $"{(currentYear % 100):D2}{((currentYear + 2) % 100):D2}";

            // Complete prefix: ENG23250000 (ENG + YearStart + YearEnd + Sequential Number)
            string completePrefix = $"{SYSTEM_PREFIX}{yearPrefix}";

            var studentCodes = await _context.Students
                .Where(s => s.StudentCode.StartsWith(completePrefix))
                .Select(s => s.StudentCode)
                .ToListAsync();

            int maxNumber = studentCodes
                .Select(code =>
                {
                    int number;
                    return int.TryParse(code.Substring(7), out number) ? number : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            // Format: ENG23250001
            return $"{completePrefix}{(maxNumber + 1):D4}";
        }
    }
}
