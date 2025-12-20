using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;
using ProjectDataManager.Contracts.IServices;
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
        var employeeService = _unitOfWork.GetRepository<IEmployeeService>();

        return employeeService.GetEmployeesAsync(term);
    }

    public Task<List<EmployeeForSelectDto>> GetForSelectHandleAsync(string term)
    {
        var employeeService = _unitOfWork.GetRepository<IEmployeeService>();

        return employeeService.GetEmployeesForSelectAsync(term);
    }
}
