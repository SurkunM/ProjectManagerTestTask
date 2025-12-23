using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class DeleteEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<DeleteEmployeeHandler> _logger;


    public DeleteEmployeeHandler(IUnitOfWork unitOfWork, ILogger<DeleteEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(int id)
    {
        var employeeService = _unitOfWork.GetService<IEmployeeService>();

        var employee = await employeeService.FindEmployeeByIdAsync(id) ?? throw new NotFoundException("Delete failed. Employee not found");

        var result = await employeeService.DeleteAndSaveChanges(employee);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));

            throw new OperationFailedException($"Delete failed. {errors}");
        }
    }
}
