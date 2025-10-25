using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class QuizQuestionRepository : Repository<QuizQuestion>, IQuizQuestionRepository
    {
        private readonly ApplicationDBContext _context;
        public QuizQuestionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
