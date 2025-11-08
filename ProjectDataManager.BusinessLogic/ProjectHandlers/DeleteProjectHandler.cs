using Microsoft.Extensions.Logging;
using ProductionChain.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class DeleteProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<DeleteProjectHandler> _logger;

    public DeleteProjectHandler(IUnitOfWork unitOfWork, ILogger<DeleteProjectHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(int id)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectsRepository.FindProjectByIdAsync(id);

            if (project is null)
            {
                throw new NotFoundException("Project not found or already deleted");
            }

            projectsRepository.Delete(project);

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
