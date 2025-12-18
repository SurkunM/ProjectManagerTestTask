using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
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
        var employeeService = _unitOfWork.GetRepository<IEmployeeService>();

        var result = await employeeService.CreateAsyncAndSaveChanges(requestDto.ToModel());

        if (!result.Succeeded)
        {
            throw new OperationFailedException($"Create failed. {result.Errors}");
        }
    }
}
