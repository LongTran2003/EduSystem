using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly ApplicationDBContext _context;
        public AnswerRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
