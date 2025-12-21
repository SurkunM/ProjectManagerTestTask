using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class AddEmployeeToProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<AddEmployeeToProjectHandler> _logger;

    public AddEmployeeToProjectHandler(IUnitOfWork unitOfWork, ILogger<AddEmployeeToProjectHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(int projectId, int[] employeesId)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeeService = _unitOfWork.GetService<IEmployeeService>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectsRepository.FindProjectByIdAsync(projectId) ?? throw new NotFoundException("Project not found");
            var employees = await employeeService.FindEmployeesByIdAsync(employeesId);

            if (employees.Count == 0)
            {
                throw new NotFoundException("No employees found");
            }

            if (employees.Count != employeesId.Length)
            {
                _logger.LogError("Some employees not found");
            }

            await projectsRepository.AddEmployeesToProject(project, employees);

            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction rolled back");

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
