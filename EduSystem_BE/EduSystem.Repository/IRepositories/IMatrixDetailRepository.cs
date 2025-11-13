using EduSystem.Models.Entities;

namespace EduSystem.Repository.IRepositories
{
    public interface IMatrixDetailRepository : IRepository<MatrixDetail>
    {
        Task<(List<MatrixDetail> matrixDetails, int totalCount)> GetMatrixDetailsAsync(
            int pageNumber,
            int pageSize,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAdmin = false,
            string? includeProperties = null);

        void Update(MatrixDetail target, MatrixDetail source);
    }
}
