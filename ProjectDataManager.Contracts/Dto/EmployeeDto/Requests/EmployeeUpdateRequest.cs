using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;

public class EmployeeUpdateRequest
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public required string LastName { get; set; }

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [StringLength(50)]
    public required string Email { get; set; }
}
