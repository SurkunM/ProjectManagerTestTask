using Microsoft.Extensions.Logging;
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

    public async Task<bool> HandleAsync(int projectId, int employeeId)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectsRepository.FindProjectByIdAsync(projectId);
            var employee = await employeesRepository.FindEmployeeByIdAsync(employeeId);

            if (project is null || employee is null)
            {
                _logger.LogError("Project (projectId: {ProjectId}) or Employee (employeeId: {EmployeeId}) not found.", projectId, employeeId);

                _unitOfWork.RollbackTransaction();

                return false;
            }

            if (await projectsRepository.CheckEmployeeProjectMembershipAsync(projectId, employeeId))
            {
                _logger.LogError("Employee {EmployeeId} is already assigned to project {ProjectId}", employeeId, projectId);

                _unitOfWork.RollbackTransaction();

                return false;
            }

            await projectsRepository.AddEmployeeToProject(project, employee);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to adding employee {EmployeeId} to project {ProjectId}. Transaction rolled back", employeeId, projectId);

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
