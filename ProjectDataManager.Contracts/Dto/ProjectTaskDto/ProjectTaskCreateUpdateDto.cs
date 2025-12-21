using ProjectDataManager.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.ProjectTaskDto;

public class ProjectTaskCreateUpdateDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;

    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }

    [Range(1, int.MaxValue)]
    public int ExecutorId { get; set; }

    public TaskStatusEnum Status { get; set; }

    public string? Comment { get; set; }

    [Range(1, 3)]
    public int Priority { get; set; }
}
