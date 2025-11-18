namespace ProjectDataManager.Contracts.Dto.Responses;

public class ResponseData
{
    public int UserId { get; set; }

    public required string UserName { get; set; }

    public required IList<string> Roles { get; set; }
}
