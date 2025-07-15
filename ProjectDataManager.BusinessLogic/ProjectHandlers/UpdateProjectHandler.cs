using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class UpdateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(ProjectCreateUpdateDto projectDto)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        projectsRepository.Update(projectDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
