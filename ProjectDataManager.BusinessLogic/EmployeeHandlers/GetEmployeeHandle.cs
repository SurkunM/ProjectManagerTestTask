using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class GetEmployeeHandle
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEmployeeHandle(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<List<EmployeeResponseDto>> HandleAsync(string term)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        return employeesRepository.GetEmployeesAsync(term);
    }

    public Task<List<EmployeeForSelectDto>> GetForSelectHandleAsync(string term)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        return employeesRepository.GetEmployeesForSelectAsync(term);
    }
}
