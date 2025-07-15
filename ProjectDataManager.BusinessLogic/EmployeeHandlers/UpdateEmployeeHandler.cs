using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.EmployeeHandlers;

public class UpdateEmployeeHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task HandleAsync(EmployeeCreateUpdateDto requestDto)
    {
        var employeesRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        employeesRepository.Update(requestDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
