namespace ProjectDataManager.Contracts.Dto.Responses;

public class EmployeeLoginResponse
{
    public required string Token { get; set; }

    public required ResponseData UserData { get; set; }
}
