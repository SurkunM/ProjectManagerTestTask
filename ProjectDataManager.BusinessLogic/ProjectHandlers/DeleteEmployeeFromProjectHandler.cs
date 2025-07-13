using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class DeleteEmployeeFromProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeFromProjectHandler(IUnitOfWork unitOfWork)
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
