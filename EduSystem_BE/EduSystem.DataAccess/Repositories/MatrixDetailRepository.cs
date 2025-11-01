using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.Repositories
{
    public class MatrixDetailRepository : Repository<MatrixDetail>, IMatrixDetailRepository
    {
        private readonly ApplicationDBContext _context;

        public MatrixDetailRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
