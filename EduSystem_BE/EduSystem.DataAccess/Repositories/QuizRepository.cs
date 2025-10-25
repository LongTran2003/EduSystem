using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        private readonly ApplicationDBContext _context;
        public QuizRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
