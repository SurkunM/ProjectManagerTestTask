namespace ProjectDataManager.Contracts.Dto.Requests;

public class EmployeeLoginRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}
