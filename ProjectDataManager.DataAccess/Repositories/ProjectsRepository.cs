using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectDataManager.DataAccess.Repositories;

public class ProjectsRepository : BaseEfRepository<Project>, IProjectsRepository
{
    public ProjectsRepository(ProjectDataManagerDbContext dbContext, ILogger<ProjectsRepository> logger) : base(dbContext) { }

    public async Task<List<ProjectResponseDto>> GetProjectsAsync(GetProjectsQueryParameters queryParameters)
    {
        var querySbSet = DbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(queryParameters.Term))
        {
            queryParameters.Term = queryParameters.Term.Trim();
            querySbSet = querySbSet.Where(c => c.Name.Contains(queryParameters.Term)
                || c.ContractorCompany.Contains(queryParameters.Term)
                || c.CustomerCompany.Contains(queryParameters.Term));
        }

        var orderByExpression = CreateSortExpression(queryParameters.SortBy);

        var orderedQuery = queryParameters.IsDescending
            ? querySbSet.OrderByDescending(orderByExpression)
            : querySbSet.OrderBy(orderByExpression);

        return await orderedQuery
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                CustomerCompany = p.CustomerCompany,
                ContractorCompany = p.ContractorCompany,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Priority = p.Priority,
                ProjectManagerFullName = $"{p.ProjectManager.LastName} {p.ProjectManager.FirstName} {p.ProjectManager.MiddleName ?? string.Empty}"
            })
            .ToListAsync();
    }

    public Task<bool> CheckEmployeeProjectMembershipAsync(int projectId, int employeeId)
    {
        return DbContext.Set<ProjectEmployee>().AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }

    public Task<Project?> FindProjectByIdAsync(int id)
    {
        return DbSet.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<ProjectEmployee?> FindProjectEmployeeByIdAsync(int projectId, int employeeId)
    {
        return DbContext.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }

    public Task AddEmployeeToProject(Project project, Employee employee)
    {
        var projectEmployee = new ProjectEmployee
        {
            ProjectId = project.Id,
            Project = project,
            EmployeeId = employee.Id,
            Employee = employee
        };

        return DbContext.ProjectEmployees.AddAsync(projectEmployee).AsTask();
    }

    public void RemoveEmployeeFromProject(ProjectEmployee projectEmployee)
    {
        if (DbContext.Entry(projectEmployee).State == EntityState.Detached)
        {
            DbContext.ProjectEmployees.Attach(projectEmployee);
        }

        DbContext.ProjectEmployees.Remove(projectEmployee);
    }

    private static Expression<Func<Project, object>> CreateSortExpression(string propertyName)
    {
        try
        {
            return GetPropertyExpression(propertyName);
        }
        catch (Exception)
        {
            return GetPropertyExpression("Name");
        }
    }

    private static Expression<Func<Project, object>> GetPropertyExpression(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(Project), "p");

        var property = typeof(Project).GetProperty(propertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
          ?? throw new ArgumentException($"Invalid property name: {propertyName}");

        var access = Expression.MakeMemberAccess(parameter, property);

        return Expression.Lambda<Func<Project, object>>(Expression.Convert(access, typeof(object)), parameter);
    }
}
