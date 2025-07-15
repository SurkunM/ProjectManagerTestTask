using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectsRepository : IRepository<Project>
{
    Task<List<ProjectResponseDto>> GetProjectsAsync(GetProjectsQueryParameters queryParameters);

    Task<Project?> FindProjectByIdAsync(int id);

    Task<ProjectEmployee?> FindProjectEmployeeByIdAsync(int projectId, int employeeId);

    Task<bool> CheckEmployeeProjectMembershipAsync(int projectId, int employeeId);

    Task AddEmployeeToProject(Project project, Employee employee);

    void RemoveEmployeeFromProject(ProjectEmployee projectEmployee);
}
