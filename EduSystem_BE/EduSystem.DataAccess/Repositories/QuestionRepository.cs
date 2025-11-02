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
                    "content" => query.Where(s => s.Content.Contains(filterQuery)),
                    "questiontype" => query.Where(s => s.QuestionType != null && s.QuestionType.Contains(filterQuery)),
                    "level" => query.Where(s => s.Level != null && s.Level.Contains(filterQuery)),
                    "skilltype" => query.Where(s => s.SkillType != null && s.SkillType.Contains(filterQuery)),
                    "englishlevel" => query.Where(s =>
                        s.EnglishLevel != null && s.EnglishLevel.Contains(filterQuery)),
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
                    "content" => query.OrderBy(q => q.Content),
                    "contentdesc" => query.OrderByDescending(q => q.Content),
                    "questiontype" => query.OrderBy(q => q.QuestionType),
                    "level" => query.OrderBy(q => q.Level),
                    "skilltype" => query.OrderBy(q => q.SkillType),
                    "englishlevel" => query.OrderBy(q => q.EnglishLevel),
                    "createdtime" => query.OrderByDescending(q => q.CreatedTime),
                    _ => query.OrderByDescending(q => q.CreatedTime)
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
