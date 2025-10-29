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
            var query = _context.Questions.AsQueryable();

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
                    "questiontext" => query.Where(s => s.QuestionText.Contains(filterQuery)),
                    "questiontype" => query.Where(s => s.QuestionType.Contains(filterQuery)),
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
                    "questiontext" => query.OrderBy(q => q.QuestionText),
                    "questiontextdesc" => query.OrderByDescending(q => q.QuestionText),
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
