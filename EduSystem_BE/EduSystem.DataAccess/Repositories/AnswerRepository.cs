using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly ApplicationDBContext _context;
        public AnswerRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Answer> answers, int totalAnswers)> GetAnswersAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy, 
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.Answers.Include(a => a.Question).AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(a => a.Status == StaticOperationStatus.BaseEntity.Active);
            }

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                filterOn = filterOn.Trim().ToLower();
                filterQuery = filterQuery.Trim();

                query = filterOn switch
                {
                    "content" => query.Where(a => a.Content.Contains(filterQuery)),
                    "iscorrect" => query.Where(a => a.IsCorrect.ToString().Contains(filterQuery)),
                    "explanation" => query.Where(a => a.Explanation != null && a.Explanation.Contains(filterQuery)),
                    "status" => query.Where(a => a.Status.Contains(filterQuery)),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                sortBy = sortBy.Trim().ToLower();

                query = sortBy switch
                {
                    "content" => query.OrderBy(a => a.Content),
                    "content_desc" => query.OrderByDescending(a => a.Content),
                    "createdtime" => query.OrderBy(a => a.CreatedTime),
                    "createdtime_desc" => query.OrderByDescending(a => a.CreatedTime),
                    _ => query.OrderByDescending(a => a.CreatedTime)
                };
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            var totalAnswers = await query.CountAsync();

            var answers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (answers, totalAnswers);
        }

        public void Update(Answer target, Answer source)
        {
            _context.Set<Answer>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }

        public void Update(Answer answer)
        {
            _context.Set<Answer>().Attach(answer);
            _context.Entry(answer).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid questionId)
        {
            return await _context.Answers
                .Where(a => a.QuestionId == questionId && a.Status != StaticOperationStatus.BaseEntity.Deleted)
                .ToListAsync();
        }
    }
}
