using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.EmployeeDto;

public class EmployeeCreateUpdateDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [Required]
    [StringLength(50)]
    public required string Email { get; set; }
}
