using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class UpdateEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeHandler(IUnitOfWork unitOfWork)
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
