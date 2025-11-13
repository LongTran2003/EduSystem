using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class StudentAnswerRepository : Repository<StudentAnswer>, IStudentAnswerRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentAnswerRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<StudentAnswer> items, int totalCount)> GetStudentAnswersAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin,
            string? includeProperties = null)
        {
            IQueryable<StudentAnswer> query = _context.StudentAnswers;

            if (!isAdmin)
            {
                query = query.Where(sa => sa.Status != StaticOperationStatus.BaseEntity.Deleted);
            }

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.ToLower())
                {
                    case "attemptid":
                        if (Guid.TryParse(filterQuery, out Guid attemptId))
                        {
                            query = query.Where(sa => sa.AttemptId == attemptId);
                        }
                        break;
                    case "questionid":
                        if (Guid.TryParse(filterQuery, out Guid questionId))
                        {
                            query = query.Where(sa => sa.QuestionId == questionId);
                        }
                        break;
                    case "iscorrect":
                        if (bool.TryParse(filterQuery, out bool isCorrect))
                        {
                            query = query.Where(sa => sa.IsCorrect == isCorrect);
                        }
                        break;
                    case "status":
                        query = query.Where(sa => sa.Status == filterQuery);
                        break;
                }
            }

            // Include properties
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            var totalCount = await query.CountAsync();

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "score":
                        query = query.OrderBy(sa => sa.Score);
                        break;
                    case "score_desc":
                        query = query.OrderByDescending(sa => sa.Score);
                        break;
                    case "createdtime":
                        query = query.OrderBy(sa => sa.CreatedTime);
                        break;
                    case "createdtime_desc":
                        query = query.OrderByDescending(sa => sa.CreatedTime);
                        break;
                    default:
                        query = query.OrderBy(sa => sa.CreatedTime);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(sa => sa.CreatedTime);
            }

            // Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<StudentAnswer>> GetAnswersByAttemptIdAsync(Guid attemptId)
        {
            return await _context.StudentAnswers
                .Where(sa => sa.AttemptId == attemptId && sa.Status != StaticOperationStatus.BaseEntity.Deleted)
                .Include(sa => sa.Question)
                .Include(sa => sa.Answer)
                .OrderBy(sa => sa.QuestionId)
                .ToListAsync();
        }

        public async Task<StudentAnswer?> GetAnswerByAttemptAndQuestionAsync(Guid attemptId, Guid questionId)
        {
            return await _context.StudentAnswers
                .Include(sa => sa.Question)
                .Include(sa => sa.Answer)
                .FirstOrDefaultAsync(sa => sa.AttemptId == attemptId &&
                                          sa.QuestionId == questionId &&
                                          sa.Status != StaticOperationStatus.BaseEntity.Deleted);
        }

        public void Update(StudentAnswer target, StudentAnswer source)
        {
            _context.Set<StudentAnswer>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
