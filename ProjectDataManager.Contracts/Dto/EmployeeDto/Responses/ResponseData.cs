namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;

public class ResponseData
{
    public int UserId { get; set; }

    public required string UserName { get; set; }

    public required IList<string> Roles { get; set; }
}
