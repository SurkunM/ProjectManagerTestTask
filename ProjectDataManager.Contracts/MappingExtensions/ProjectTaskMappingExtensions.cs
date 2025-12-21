using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.MappingExtensions;

public static class ProjectTaskMappingExtensions
{
    public static ProjectTask ToModel(this ProjectTaskCreateUpdateDto taskUpdateDto, Project project, Employee executor, Employee author)
    {
        return new ProjectTask
        {
            Id = taskUpdateDto.Id,
            Author = author,
            Name = taskUpdateDto.Name,
            Project = project,
            Executor = executor,
            Status = taskUpdateDto.Status,
            Comment = taskUpdateDto.Comment,
            Priority = taskUpdateDto.Priority
        };
    }
}
