using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;

public class EmployeeCreateRequest
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    [StringLength(50)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public required string LastName { get; set; }

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [StringLength(50)]
    public required string Email { get; set; }

    [MinLength(6)]
    [MaxLength(50)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$")]
    public required string Password { get; set; }
}
