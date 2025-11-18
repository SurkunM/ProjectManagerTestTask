namespace ProjectDataManager.Contracts.Dto.EmployeeDto;

public class EmployeeForSelectDto
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }
}
