namespace ProjectDataManager.Contracts.Dto.ProjectDto;

public class ProjectRemoveAddEmployeesDto
{
    public int ProjectId { get; set; }

    public int[] EmployeesId { get; set; } = [];
}
