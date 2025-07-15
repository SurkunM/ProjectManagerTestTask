using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.MappingExtensions;

public static class ProjectMappingExtensions
{
    public static Project ToModel(this ProjectCreateUpdateDto projectDto, Employee manager)
    {
        return new Project
        {
            Id = projectDto.Id,
            Name = projectDto.Name,
            CustomerCompany = projectDto.CustomerCompany,
            ContractorCompany = projectDto.ContractorCompany,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            Priority = projectDto.Priority,
            ProjectManagerId = projectDto.ProjectManagerId,
            ProjectManager = manager
        };
    }

    public static ProjectCreateUpdateDto ToDto(this Project project)
    {
        return new ProjectCreateUpdateDto
        {
            Id = project.Id,
            Name = project.Name,
            CustomerCompany = project.CustomerCompany,
            ContractorCompany = project.ContractorCompany,
            ProjectManagerId = project.ProjectManagerId
        };
    }
}
