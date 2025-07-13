using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> FindProjectByIdAsync(int id);
}
