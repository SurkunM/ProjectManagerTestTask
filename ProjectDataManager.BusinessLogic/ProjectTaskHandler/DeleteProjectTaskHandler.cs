using Microsoft.Extensions.Logging;
using ProductionChain.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class DeleteProjectTaskHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<DeleteProjectTaskHandler> _logger;


    public DeleteProjectTaskHandler(IUnitOfWork unitOfWork, ILogger<DeleteProjectTaskHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(int id)
    {
        var projectTasksRepository = _unitOfWork.GetRepository<IProjectTasksRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var task = await projectTasksRepository.FindTaskByIdAsync(id) ?? throw new NotFoundException("Task not found or already deleted");
            projectTasksRepository.Delete(task);

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
