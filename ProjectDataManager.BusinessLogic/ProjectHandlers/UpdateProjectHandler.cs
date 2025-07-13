using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class UpdateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync()
    {
        var repository = _unitOfWork.GetRepository<IProjectRepository>();

        await repository.FindProjectByIdAsync(2);

        return true;
    }
}
