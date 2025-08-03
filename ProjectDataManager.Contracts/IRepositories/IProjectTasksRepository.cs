using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectTasksRepository : IRepository<ProjectTask>
{
    Task<ProjectTask?> FindTaskByIdAsync(int id);

    Task<List<ProjectTaskResponseDto>> GetTasksAsync(string term);
}
