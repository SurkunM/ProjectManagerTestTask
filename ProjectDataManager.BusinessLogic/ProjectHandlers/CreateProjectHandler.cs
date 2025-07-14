using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class CreateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(ProjectCreateUpdateDto projectDto)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectRepository>();

        await projectsRepository.CreateAsync(projectDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
