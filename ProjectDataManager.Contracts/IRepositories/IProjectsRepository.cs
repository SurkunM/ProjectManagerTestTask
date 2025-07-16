using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectsRepository : IRepository<Project>
{
    Task<List<ProjectResponseDto>> GetProjectsAsync(GetProjectsQueryParameters queryParameters);

    Task<Project?> FindProjectByIdAsync(int id);

    Task<List<ProjectEmployee>> FindProjectEmployeeByIdAsync(int projectId, int[] employeeId);

    Task AddEmployeesToProject(Project project, List<Employee> employee);

    void RemoveEmployeesFromProject(List<ProjectEmployee> projectEmployeesList);
}
