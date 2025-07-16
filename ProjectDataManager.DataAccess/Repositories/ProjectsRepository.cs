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

    public Task<Project?> FindProjectByIdAsync(int id)
    {
        return DbSet.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<ProjectEmployee>> FindProjectEmployeeByIdAsync(int projectId, int[] employeeId)
    {
        return DbContext.ProjectEmployees
            .Where(pe => pe.ProjectId == projectId && employeeId.Contains(pe.EmployeeId))
            .ToListAsync();
    }

    public Task AddEmployeesToProject(Project project, List<Employee> employees)
    {
        var existingEmployeeIds = project.ProjectEmployees
            .Select(pe => pe.EmployeeId)
            .ToHashSet();

        var projectEmployees = employees
            .Where(e => !existingEmployeeIds.Contains(e.Id))
            .Select(e => new ProjectEmployee
            {
                ProjectId = project.Id,
                Project = project,
                EmployeeId = e.Id,
                Employee = e
            });

        return DbContext.ProjectEmployees.AddRangeAsync(projectEmployees);
    }

    public void RemoveEmployeesFromProject(List<ProjectEmployee> projectEmployeesList)
    {
        foreach (var projectEmployee in projectEmployeesList)
        {
            if (DbContext.Entry(projectEmployee).State == EntityState.Detached)
            {
                DbContext.ProjectEmployees.Attach(projectEmployee);
            }
        }

        DbContext.ProjectEmployees.RemoveRange(projectEmployeesList);
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
