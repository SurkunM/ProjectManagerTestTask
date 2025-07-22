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
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var employee = await employeesRepository.FindEmployeeByIdAsync(id);

            if (employee is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            employeesRepository.Delete(employee);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete employee {EmployeeId}. Transaction rolled back", id);

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
