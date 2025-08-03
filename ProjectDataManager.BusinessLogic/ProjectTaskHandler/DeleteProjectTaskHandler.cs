using Microsoft.Extensions.Logging;
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

    public async Task<bool> HandleAsync(int id)
    {
        var projectTasksRepository = _unitOfWork.GetRepository<IProjectTasksRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var task = await projectTasksRepository.FindTaskByIdAsync(id);

            if (task is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            projectTasksRepository.Delete(task);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete task {taskId}. Transaction rolled back", id);

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
