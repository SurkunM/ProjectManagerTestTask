namespace ProjectDataManager.Model;

public class ProjectEmployee
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public virtual required Project Project { get; set; }

    public int EmployeeId { get; set; }

    public virtual required Employee Employee { get; set; }
}
