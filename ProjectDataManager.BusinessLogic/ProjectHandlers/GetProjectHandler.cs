using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class GetProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<ProjectResponseDto>> HandleAsync(GetProjectsQueryParameters queryParameters)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        return projectsRepository.GetProjectsAsync(queryParameters);
    }
}
