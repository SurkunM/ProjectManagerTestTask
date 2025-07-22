using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Repositories;

public class ProjectTasksRepository : BaseEfRepository<ProjectTask>, IProjectTasksRepository
{
    public ProjectTasksRepository(ProjectDataManagerDbContext dbContext) : base(dbContext)
    {

    }

    public Task<ProjectTask> FindTaskByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectTask> GetTasks()
    {
        throw new NotImplementedException();
    }
}
