namespace ProjectDataManager.Contracts.Dto.ProjectTaskDto;

public class ProjectTaskCreateUpdateDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int ProjectId { get; set; }

    public int AuthorId { get; set; }

    public int ExecutorId { get; set; }

    public TaskStatus Status { get; set; }

    public string? Comment { get; set; }

    public int Priority { get; set; }
}
