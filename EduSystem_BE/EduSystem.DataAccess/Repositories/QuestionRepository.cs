using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDBContext _context;
        public QuestionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
