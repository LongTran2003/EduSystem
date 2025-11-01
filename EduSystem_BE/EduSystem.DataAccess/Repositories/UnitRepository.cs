using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        private readonly ApplicationDBContext _context;

        public UnitRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
