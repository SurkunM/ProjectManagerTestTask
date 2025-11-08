using Microsoft.Extensions.Logging;
using ProductionChain.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class DeleteEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<DeleteEmployeeHandler> _logger;


    public DeleteEmployeeHandler(IUnitOfWork unitOfWork, ILogger<DeleteEmployeeHandler> logger)
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
                throw new NotFoundException("Employee not found or already deleted");
            }

            employeesRepository.Delete(employee);

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
