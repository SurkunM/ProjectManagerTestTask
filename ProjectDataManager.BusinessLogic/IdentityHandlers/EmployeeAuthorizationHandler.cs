using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.BusinessLogic.IdentityHandlers;

public class EmployeeAuthorizationHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly UserManager<Employee> _userManager;

    private readonly ILogger<EmployeeAuthorizationHandler> _logger;

    public EmployeeAuthorizationHandler(UserManager<Employee> userManager, IUnitOfWork unitOfWork, ILogger<EmployeeAuthorizationHandler> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(EmployeeRegisterRequest employeeRegisterRequest)
    {
        if (!employeeRegisterRequest.IsValidRole())
        {
            throw new InvalidRoleException(employeeRegisterRequest.Role);
        }

        var employeeService = _unitOfWork.GetRepository<IEmployeeService>();

        try
        {
            _unitOfWork.BeginTransaction();

            var employee = await employeeService.FindEmployeeByIdAsync(employeeRegisterRequest.EmployeeId) ?? throw new NotFoundException("Employee not found");

            var result = await _userManager.CreateAsync(employee, employeeRegisterRequest.Password);

            if (!result.Succeeded)
            {
                throw new RegistrationFailedException($"Registration failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(employee, employeeRegisterRequest.Role);

            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();

            _logger.LogError(ex, "Transaction rolled back");

            throw;
        }

    }
}
