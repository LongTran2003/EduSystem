using EduSystem.Models.Entities;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using EduSystem.Utilities.Contants;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.Repositories
{
    public class MatrixDetailRepository : Repository<MatrixDetail>, IMatrixDetailRepository
    {
        private readonly ApplicationDBContext _context;

        public MatrixDetailRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<MatrixDetail> matrixDetails, int totalCount)> GetMatrixDetailsAsync(
            int pageNumber,
            int pageSize,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAdmin = false,
            string? includeProperties = null)
        {
            var query = _context.MatrixDetails.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(md => md.Status == StaticOperationStatus.BaseEntity.Active);
            }

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "level" => query.Where(md => md.Level.ToString().Contains(filterQuery)),
                    "questiontype" => query.Where(md => md.QuestionType.ToString().Contains(filterQuery)),
                    "skilltype" => query.Where(md => md.SkillType != null &&
                                                    md.SkillType.Contains(filterQuery)),
                    "status" => query.Where(md => md.Status.Contains(filterQuery)),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            var totalCount = await query.CountAsync();

            query = sortBy?.ToLower() switch
            {
                "level" => query.OrderBy(md => md.Level),
                "level_desc" => query.OrderByDescending(md => md.Level),
                "questioncount" => query.OrderBy(md => md.QuestionCount),
                "questioncount_desc" => query.OrderByDescending(md => md.QuestionCount),
                "scoreperquestion" => query.OrderBy(md => md.ScorePerQuestion),
                "scoreperquestion_desc" => query.OrderByDescending(md => md.ScorePerQuestion),
                _ => query.OrderByDescending(md => md.CreatedTime)
            };

            var matrixDetails = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (matrixDetails, totalCount);
        }

        public void Update(MatrixDetail target, MatrixDetail source)
        {
            _context.Set<MatrixDetail>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
