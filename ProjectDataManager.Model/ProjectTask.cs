using ProjectDataManager.Model.Enums;

namespace ProjectDataManager.Model;

public class ProjectTask
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int ProjectId { get; set; }

    public virtual required Project Project { get; set; }

    public int AuthorId { get; set; }

    public virtual required Employee Author { get; set; }

    public int ExecutorId { get; set; }

    public virtual required Employee Executor { get; set; }

    public TaskStatusEnum Status { get; set; }

    public string? Comment { get; set; }

    public int Priority { get; set; }
}
