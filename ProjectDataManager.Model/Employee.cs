using Microsoft.AspNetCore.Identity;

namespace ProjectDataManager.Model;

public class Employee : IdentityUser<int>
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }

    public int? ManagerId { get; set; }

    public virtual Employee? Manager { get; set; }

    public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

    public virtual ICollection<Project> ManagedProjects { get; set; } = new List<Project>();

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

    public virtual ICollection<ProjectTask> AuthoredTasks { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectTask> ExecutingTasks { get; set; } = new List<ProjectTask>();
}
