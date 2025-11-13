using EduSystem.Models.Entities;
using EduSystem.Models.Enums;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDBContext _context;

        public TeacherRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Teacher> Teachers, int TotalTeachers)> GetTeachersAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null)
        {
            var query = _context.Teachers.AsQueryable();

            // Filter by status if not admin
            if (!isAdmin)
            {
                query = query.Where(t => t.Status == TeacherStatus.Active);
            }

            // Apply filters
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "fullname" => query.Where(t => t.ApplicationUser.FullName.Contains(filterQuery)),
                    "email" => query.Where(t => t.ApplicationUser.Email != null && t.ApplicationUser.Email.Contains(filterQuery)),
                    "phonenumber" => query.Where(t => t.ApplicationUser.PhoneNumber != null && t.ApplicationUser.PhoneNumber.Contains(filterQuery)),
                    "teachercode" => query.Where(t => t.TeacherCode.Contains(filterQuery)),
                    "status" => Enum.TryParse<TeacherStatus>(filterQuery, true, out var status)
                        ? query.Where(t => t.Status == status)
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

            // Get total count
            int totalTeachers = await query.CountAsync();

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "fullname" => query.OrderBy(t => t.ApplicationUser.FullName),
                "fullname_desc" => query.OrderByDescending(t => t.ApplicationUser.FullName),
                "email" => query.OrderBy(t => t.ApplicationUser.Email),
                "email_desc" => query.OrderByDescending(t => t.ApplicationUser.Email),
                "teachercode" => query.OrderBy(t => t.TeacherCode),
                "teachercode_desc" => query.OrderByDescending(t => t.TeacherCode),
                "status" => query.OrderBy(t => t.Status),
                "status_desc" => query.OrderByDescending(t => t.Status),
                "teachingexperience" => query.OrderBy(t => t.TeachingExperience),
                "teachingexperience_desc" => query.OrderByDescending(t => t.TeachingExperience),
                _ => query.OrderBy(t => t.ApplicationUser.FullName)
            };

            // Apply pagination
            var teachers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (teachers, totalTeachers);
        }

        public void Update(Teacher teacher)
        {
            _context.Teachers.Attach(teacher);
            _context.Entry(teacher).State = EntityState.Modified;
        }

        public async Task<string> GetNextTeacherCodeAsync()
        {

            // Lấy 2 số cuối của năm hiện tại
            int currentYear = DateTime.Now.Year % 100;
            string prefix = $"S{currentYear:D2}-";

            // Lấy danh sách các phần số của TeacherCode (sau dấu gạch)
            var codeParts = await _context.Teachers
                .Where(s => s.TeacherCode.StartsWith(prefix))
                .Select(s => s.TeacherCode.Substring(prefix.Length))
                .ToListAsync();

            // Chuyển đổi chuỗi sang số và tìm số lớn nhất
            int maxNumber = codeParts
                .Select(codePart =>
                {
                    int number;
                    return int.TryParse(codePart, out number) ? number : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            int nextNumber = maxNumber + 1;
            return $"{prefix}{nextNumber:D4}";
        }

        public async Task<string> CalculateTeachingExperienceAsync(int initialYears)
        {
            // Tự động tăng 1 năm kinh nghiệm cho mỗi năm kể từ khi đăng ký
            // Có thể thêm logic phức tạp hơn nếu cần
            return initialYears switch
            {
                0 => "Less than a year",
                1 => "1 year",
                _ => $"{initialYears} years"
            };
        }
    }
}
