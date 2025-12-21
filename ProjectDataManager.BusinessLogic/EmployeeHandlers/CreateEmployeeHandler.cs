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

    public async Task HandleAsync(EmployeeCreateRequest requestDto)
    {
        var employeeService = _unitOfWork.GetService<IEmployeeService>();

        var result = await employeeService.CreateAsyncAndSaveChanges(requestDto.ToModel(), requestDto.Password);

        if (!result.Succeeded)
        {
            var errorsMassage = string.Join("; ", result.Errors.Select(e => e.Description));

            throw new OperationFailedException($"Create failed. {errorsMassage}");
        }
    }
}
