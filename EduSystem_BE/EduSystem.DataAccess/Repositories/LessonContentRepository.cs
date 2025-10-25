using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class LessonContentRepository : Repository<LessonContent>, ILessonContentRepository
    {
        private readonly ApplicationDBContext _context;
        public LessonContentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
