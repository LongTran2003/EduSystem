using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDBContext _context;
        public SubjectRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
