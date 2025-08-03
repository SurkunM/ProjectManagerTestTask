using Microsoft.EntityFrameworkCore;
using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Repositories;

public class ProjectTasksRepository : BaseEfRepository<ProjectTask>, IProjectTasksRepository
{
    public ProjectTasksRepository(ProjectDataManagerDbContext dbContext) : base(dbContext) { }

    public Task<List<ProjectTaskResponseDto>> GetTasksAsync(string term)
    {
        return DbSet
            .Where(t => t.Name.Contains(term) || t.Project.Name.Contains(term) || t.Executor.FirstName.Contains(term))
            .Select(t => new ProjectTaskResponseDto
            {
                Id = t.Id,
                ProjectName = t.Project.Name,
                Executor = $"{t.Executor.LastName} {t.Executor.FirstName} {t.Executor.MiddleName ?? string.Empty}",
                Status = t.Status.ToString(),
                Comments = t.Comment,
                Priority = t.Priority
            })
            .ToListAsync();
    }

    public Task<ProjectTask?> FindTaskByIdAsync(int id)
    {
        return DbSet.FirstOrDefaultAsync(t => t.Id == id);
    }
}
