using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        private readonly ApplicationDBContext _context;
        public QuizRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Quiz> quizzes, int totalQuizzes)> GetQuizzesAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy,
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.Quizzes.AsQueryable();

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
            var totalQuizzes = await query.CountAsync();

            // Apply pagination
            var quizzes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (quizzes, totalQuizzes);
        }

        public void Update(Quiz target, Quiz source)
        {
            _context.Set<Quiz>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
