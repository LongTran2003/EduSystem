using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class QuizAttemptRepository : Repository<QuizAttempt>, IQuizAttemptRepository
    {
        private readonly ApplicationDBContext _context;

        public QuizAttemptRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
