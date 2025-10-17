using EduSystem.Models.Entities;

namespace EduSystem.DataAccess.IRepositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        void Update(Teacher teacher);
        Task<string> GetNextTeacherCodeAsync();
    }
}
