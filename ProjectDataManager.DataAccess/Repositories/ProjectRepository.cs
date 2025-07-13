using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Repositories;

public class ProjectRepository : BaseEfRepository<Project>, IProjectRepository
{
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(ProjectDataManagerDbContext dbContext, ILogger<ProjectRepository> logger) : base(dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<Project?> FindProjectByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
