using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        private readonly ApplicationDBContext _context;
        public LessonRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
