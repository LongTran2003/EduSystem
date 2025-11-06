using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDBContext _context;
        public QuestionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<(List<Question> questions, int totalQuestions)> GetQuestionsAsync(
            int pageNumber, 
            int pageSize, 
            string? filterOn, 
            string? filterQuery, 
            string? sortBy,
            bool isAdmin = false, 
            string? includeProperties = null)
        {
            var query = _context.Questions.Include(q => q.Teacher)
                .ThenInclude(t => t.ApplicationUser).AsQueryable();

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
                    "title" => query.Where(q => q.Title.Contains(filterQuery)),
                    "content" => query.Where(q => q.Content.Contains(filterQuery)),
                    "type" => query.Where(q => q.QuestionType.Contains(filterQuery)),
                    "level" => query.Where(q => q.Level.Contains(filterQuery)),
                    "skilltype" => query.Where(q => q.SkillType != null && q.SkillType.Contains(filterQuery)),
                    "englishlevel" => query.Where(q => q.EnglishLevel != null && q.EnglishLevel.Contains(filterQuery)),
                    "score" => query.Where(q => q.Score.ToString().Contains(filterQuery)),
                    "status" => query.Where(s => s.Status == StaticOperationStatus.BaseEntity.Active),
                    _ => query
                };
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy switch
                {
                    "title" => query.OrderBy(q => q.Title),
                    "title_desc" => query.OrderByDescending(q => q.Title),
                    "type" => query.OrderBy(q => q.QuestionType),
                    "type_desc" => query.OrderByDescending(q => q.QuestionType),
                    "level" => query.OrderBy(q => q.Level),
                    "level_desc" => query.OrderByDescending(q => q.Level),
                    "score" => query.OrderBy(q => q.Score),
                    "score_desc" => query.OrderByDescending(q => q.Score),
                    "createdtime" => query.OrderBy(q => q.CreatedTime),
                    "createdtime_desc" => query.OrderByDescending(q => q.CreatedTime),
                    _ => query.OrderByDescending(q => q.CreatedTime)
                };
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
            var totalQuestions = await query.CountAsync();

            // Apply pagination
            var questions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (questions, totalQuestions);
        }

        public void Update(Question target, Question source)
        {
            _context.Set<Question>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
