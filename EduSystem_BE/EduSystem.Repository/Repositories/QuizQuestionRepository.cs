using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.Repositories
{
    public class QuizQuestionRepository : Repository<QuizQuestion>, IQuizQuestionRepository
    {
        private readonly ApplicationDBContext _context;
        public QuizQuestionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<QuizQuestion?> GetQuizQuestionAsync(Guid quizId, Guid questionId)
        {
            return await _context.QuizQuestions
                .Include(qq => qq.Quiz)
                .Include(qq => qq.Question)
                .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
        }

        public async Task<IEnumerable<QuizQuestion>> GetQuestionsByQuizIdAsync(Guid quizId)
        {
            return await _context.QuizQuestions
                .Include(qq => qq.Quiz)
                .Include(qq => qq.Question)
                .Where(qq => qq.QuizId == quizId)
                .OrderBy(qq => qq.QuestionOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizQuestion>> GetQuizzesByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuizQuestions
                .Include(qq => qq.Quiz)
                .Include(qq => qq.Question)
                .Where(qq => qq.QuestionId == questionId)
                .OrderBy(qq => qq.Quiz.QuizName)
                .ToListAsync();
        }

        public async Task<bool> IsQuestionInQuizAsync(Guid quizId, Guid questionId)
        {
            return await _context.QuizQuestions
                .AnyAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
        }

        public async Task<int> GetMaxQuestionOrderAsync(Guid quizId)
        {
            var maxOrder = await _context.QuizQuestions
                .Where(qq => qq.QuizId == quizId)
                .MaxAsync(qq => (int?)qq.QuestionOrder);

            return maxOrder ?? 0;
        }

        public async Task RemoveQuestionsByQuizIdAsync(Guid quizId)
        {
            var quizQuestions = await _context.QuizQuestions
                .Where(qq => qq.QuizId == quizId)
                .ToListAsync();

            _context.QuizQuestions.RemoveRange(quizQuestions);
        }

        public void Update(QuizQuestion quizQuestion)
        {
            _context.Set<QuizQuestion>().Attach(quizQuestion);
            _context.Entry(quizQuestion).State = EntityState.Modified;
        }
    }
}
