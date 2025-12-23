namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;

public class EmployeeLoginResponse
{
    public required string Token { get; set; }

    public required EmployeeResponseData UserData { get; set; }
}
