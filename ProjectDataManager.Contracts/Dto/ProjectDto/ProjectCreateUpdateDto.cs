using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.ProjectDto;

public class ProjectCreateUpdateDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string CustomerCompany { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string ContractorCompany { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Range(1, 3)]
    public int Priority { get; set; }

    public int? ProjectManagerId { get; set; }
}
