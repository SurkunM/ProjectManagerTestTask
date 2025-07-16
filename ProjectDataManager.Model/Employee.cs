namespace ProjectDataManager.Model;

public class Employee
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string? MiddleName { get; set; }

    public required string Email { get; set; }

    public virtual ICollection<Project> ManagedProjects { get; set; } = new List<Project>();

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

    public virtual ICollection<ProjectTask> AuthoredTasks { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectTask> ExecutingTasks { get; set; } = new List<ProjectTask>();
}
