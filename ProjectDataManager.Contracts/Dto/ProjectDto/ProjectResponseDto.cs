namespace ProjectDataManager.Contracts.Dto.ProjectDto;

public class ProjectResponseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string CustomerCompany { get; set; }

    public required string ContractorCompany { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int Priority { get; set; }

    public string? ProjectManagerFullName { get; set; }
}
