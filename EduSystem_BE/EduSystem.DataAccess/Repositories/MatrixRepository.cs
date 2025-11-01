using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class MatrixRepository : Repository<Matrix>, IMatrixRepository
    {
        private readonly ApplicationDBContext _context;
        public MatrixRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
