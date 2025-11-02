using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<(List<Unit> units, int totalUnits)> GetUnitsAsync(
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isAdmin = false,
            string? includeProperties = null);
        
        void Update(Unit target, Unit source);
    }
}
