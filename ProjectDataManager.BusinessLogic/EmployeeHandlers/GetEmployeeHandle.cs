using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class GetEmployeeHandle
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEmployeeHandle(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync()
    {
        var repository = _unitOfWork.GetRepository<IEmployeeRepository>();

        await repository.FindEmployeeByIdAsync(2);

        return true;
    }
}
