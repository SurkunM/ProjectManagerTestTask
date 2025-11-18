using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.Dto.Requests;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.MappingExtensions;

public static class EmployeeMappingExtensions
{
    public static Employee ToModel(this EmployeeCreateUpdateDto crateUpdateDto)
    {
        return new Employee
        {
            Id = crateUpdateDto.Id,
            FirstName = crateUpdateDto.FirstName,
            LastName = crateUpdateDto.LastName,
            MiddleName = crateUpdateDto.MiddleName,
            Email = crateUpdateDto.Email
        };
    }

    public static EmployeeResponseDto ToDto(this Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            MiddleName = employee.MiddleName,
            Email = employee.Email
        };
    }
}