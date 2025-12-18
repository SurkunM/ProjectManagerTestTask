using Microsoft.AspNetCore.Identity;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class UpdateEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly UserManager<Employee> _userManager;

    public UpdateEmployeeHandler(IUnitOfWork unitOfWork, UserManager<Employee> userManager)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task HandleAsync(EmployeeCreateUpdateDto requestDto)
    {
        var employeeService = _unitOfWork.GetService<IEmployeeService>();

        var result = await employeeService.UpdateAndSaveChanges(requestDto);

        if (!result.Succeeded)
        {
            throw new OperationFailedException($"Update failed. {result.Errors}");
        }
    }
}
