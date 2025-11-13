using EduSystem.Models.Entities;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
{
    public class QuizAttemptRepository : Repository<QuizAttempt>, IQuizAttemptRepository
    {
        private readonly ApplicationDBContext _context;

        public QuizAttemptRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<QuizAttempt> quizAttempts, int totalCount)> GetQuizAttemptsAsync(
            int pageNumber,
            int pageSize,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAdmin = false,
            string? includeProperties = null)
        {
            var query = _context.QuizAttempts.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(qa => qa.Status != StaticOperationStatus.BaseEntity.Deleted);
            }

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "studentid" => query.Where(qa => qa.StudentId.ToString().Contains(filterQuery)),
                    "quizid" => query.Where(qa => qa.QuizId.ToString().Contains(filterQuery)),
                    "status" => query.Where(qa => qa.Status.Contains(filterQuery)),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            var totalCount = await query.CountAsync();

            query = sortBy?.ToLower() switch
            {
                "starttime" => query.OrderBy(qa => qa.StartTime),
                "starttime_desc" => query.OrderByDescending(qa => qa.StartTime),
                "score" => query.OrderBy(qa => qa.Score),
                "score_desc" => query.OrderByDescending(qa => qa.Score),
                _ => query.OrderByDescending(qa => qa.StartTime)
            };

            var quizAttempts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (quizAttempts, totalCount);
        }

        public void Update(QuizAttempt target, QuizAttempt source)
        {
            _context.Set<QuizAttempt>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
