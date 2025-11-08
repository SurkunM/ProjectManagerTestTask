using Microsoft.Extensions.Logging;
using ProductionChain.Contracts.Exceptions;
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

    public async Task<bool> HandleAsync(int projectId, int[] employeesId)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var projectEmployee = await projectsRepository.FindProjectEmployeeByIdAsync(projectId, employeesId);

            if (projectEmployee.Count == 0)
            {
                throw new NotFoundException("No employees were found to delete");
            }

            projectsRepository.RemoveEmployeesFromProject(projectEmployee);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction rolled back");

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
