namespace ProjectDataManager.Model;

public class Project
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string CustomerCompany { get; set; }

    public required string ContractorCompany { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Priority { get; set; }

    public int ProjectManagerId { get; set; }

    public required virtual Employee ProjectManager { get; set; }

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
}
