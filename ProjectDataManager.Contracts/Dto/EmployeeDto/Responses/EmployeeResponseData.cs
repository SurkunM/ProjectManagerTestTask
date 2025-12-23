namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;

public class EmployeeResponseData
{
    public int UserId { get; set; }

    public required string UserName { get; set; }

    public required IList<string> Roles { get; set; }
}
