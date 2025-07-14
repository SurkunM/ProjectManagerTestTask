using Microsoft.EntityFrameworkCore;
using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Repositories;

public class EmployeeRepository : BaseEfRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ProjectDataManagerDbContext dbContext) : base(dbContext) { }

    public Task<List<EmployeeResponseDto>> GetEmployeesAsync(string term)
    {
        var querySbSet = FilterEmployees(DbSet.AsNoTracking(), term);

        return querySbSet
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email
            })
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ThenBy(e => e.MiddleName)
            .ToListAsync();
    }

    public Task<List<EmployeeForSelectDto>> GetEmployeesForSelectAsync(string term)
    {
        var querySbSet = FilterEmployees(DbSet.AsNoTracking(), term);

        return querySbSet
            .Select(e => new EmployeeForSelectDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName
            })
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ThenBy(e => e.MiddleName)
            .ToListAsync();
    }

    public Task<Employee?> FindEmployeeByIdAsync(int id)
    {
        return DbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    private static IQueryable<Employee> FilterEmployees(IQueryable<Employee> query, string? term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query;
        }

        term = term.Trim();

        return query.Where(c => c.FirstName.Contains(term)
            || c.LastName.Contains(term)
            || (c.MiddleName != null && c.MiddleName.Contains(term)));
    }
}
