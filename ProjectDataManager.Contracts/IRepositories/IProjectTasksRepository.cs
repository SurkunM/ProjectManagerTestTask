using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectTasksRepository : IRepository<ProjectTask>
{
    Task<ProjectTask> FindTaskByIdAsync(int id);

    Task<ProjectTask> GetTasks();
}
