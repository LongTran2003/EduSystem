using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IMatrixRepository : IRepository<Matrix>
    {
        Task<(List<Matrix> matrices, int totalMatrices)> GetMatricesAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null);

        void Update(Matrix target, Matrix source);
    }
}
