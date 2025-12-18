namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;

public class EmployeeLoginRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}
