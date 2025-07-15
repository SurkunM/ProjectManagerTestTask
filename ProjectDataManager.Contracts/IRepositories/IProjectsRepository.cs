using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IProjectsRepository : IRepository<Project>
{
    Task<Project?> FindProjectByIdAsync(int id);

    Task<List<ProjectResponseDto>> GetProjectsAsync(GetProjectsQueryParameters queryParameters);


}
