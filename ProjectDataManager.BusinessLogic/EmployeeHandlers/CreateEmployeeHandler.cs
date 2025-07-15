using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class CreateEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task HandleAsync(EmployeeCreateUpdateDto requestDto)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        await employeesRepository.CreateAsync(requestDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
