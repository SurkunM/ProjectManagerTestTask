namespace ProjectDataManager.Contracts.Dto.ProjectTaskDto;

public class ProjectTaskResponseDto
{
    public int Id { get; set; }

    public required string ProjectName { get; set; }

    public required string Executor { get; set; }

    public required string Status { get; set; }

    public string? Comments { get; set; }

    public int Priority { get; set; }
}
