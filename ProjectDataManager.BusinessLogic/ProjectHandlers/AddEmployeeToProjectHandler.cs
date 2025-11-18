using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
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
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectsRepository.FindProjectByIdAsync(projectId);
            var employees = await employeesRepository.FindEmployeesByIdAsync(employeesId);

            if (project is null)
            {
                throw new NotFoundException("Project not found");
            }

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
