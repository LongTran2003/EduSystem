using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class StudentAnswerRepository : Repository<StudentAnswer>, IStudentAnswerRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentAnswerRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
