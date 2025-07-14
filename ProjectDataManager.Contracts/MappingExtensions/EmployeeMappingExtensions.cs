using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.MappingExtensions;

public static class EmployeeMappingExtensions
{
    public static Employee ToModel(this EmployeeCreateUpdateDto contactDto)
    {
        return new Employee
        {
            Id = contactDto.Id,
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            MiddleName = contactDto.MiddleName,
            Email = contactDto.Email
        };
    }

    public static EmployeeResponseDto ToDto(this Employee contact)
    {
        return new EmployeeResponseDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            MiddleName = contact.MiddleName,
            Email = contact.Email
        };
    }
}