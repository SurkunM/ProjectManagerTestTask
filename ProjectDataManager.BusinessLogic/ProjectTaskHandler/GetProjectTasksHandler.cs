using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class GetProjectTasksHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectTasksHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<List<EmployeeResponseDto>> HandleAsync(string term)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        return employeesRepository.GetEmployeesAsync(term);
    }
}
