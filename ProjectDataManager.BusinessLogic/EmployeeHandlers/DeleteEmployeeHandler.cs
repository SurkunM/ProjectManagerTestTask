using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class DeleteEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> HandleAsync(int id)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeeRepository>();

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
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
