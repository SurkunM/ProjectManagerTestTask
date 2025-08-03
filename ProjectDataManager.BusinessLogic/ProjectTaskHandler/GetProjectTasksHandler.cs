using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class GetProjectTasksHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectTasksHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<List<ProjectTaskResponseDto>> HandleAsync(string term)
    {
        var projectTasksRepository = _unitOfWork.GetRepository<IProjectTasksRepository>();

        return projectTasksRepository.GetTasksAsync(term);
    }
}
