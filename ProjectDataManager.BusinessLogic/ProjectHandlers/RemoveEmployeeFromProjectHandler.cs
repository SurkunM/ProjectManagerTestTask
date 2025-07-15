using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class RemoveEmployeeFromProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<RemoveEmployeeFromProjectHandler> _logger;
    public RemoveEmployeeFromProjectHandler(IUnitOfWork unitOfWork, ILogger<RemoveEmployeeFromProjectHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> HandleAsync(int projectId, int employeeId)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var projectEmployee = await projectsRepository.FindProjectEmployeeByIdAsync(projectId, employeeId);

            if (projectEmployee == null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            projectsRepository.RemoveEmployeeFromProject(projectEmployee);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove employee {EmployeeId} from project {ProjectId}. Transaction rolled back", employeeId, projectId);

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
