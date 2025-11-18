using ProjectDataManager.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.Requests;

public class EmployeeRegisterRequest
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsValidRole() => Enum.IsDefined(typeof(Roles), Role);
}
