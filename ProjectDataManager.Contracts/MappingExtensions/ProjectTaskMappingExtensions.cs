using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.MappingExtensions;

public static class ProjectTaskMappingExtensions
{
    public static ProjectTask ToModel(this ProjectTaskCreateUpdateDto createUpdateDto, Project project, Employee executor, Employee author)
    {
        return new ProjectTask
        {
            Author = author,
            Name = createUpdateDto.Name,
            Project = project,
            Executor = executor,
            Status = createUpdateDto.Status,
            Comment = createUpdateDto.Comment,
            Priority = createUpdateDto.Priority
        };
    }
}
