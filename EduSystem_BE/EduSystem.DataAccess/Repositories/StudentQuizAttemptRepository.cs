using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class StudentQuizAttemptRepository : Repository<StudentQuizAttempt>, IStudentQuizAttemptRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentQuizAttemptRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
