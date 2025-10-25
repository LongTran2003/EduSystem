using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class StudentProgressRepository : Repository<StudentProgress>, IStudentProgressRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentProgressRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
