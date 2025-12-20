using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Services;

public class EmployeeService : IEmployeeService
{
    private readonly UserManager<Employee> _userManager;

    public EmployeeService(UserManager<Employee> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }
    public Task<List<EmployeeResponseDto>> GetEmployeesAsync(string term)
    {
        return _userManager.Users
            .AsNoTracking()
            .Where(e => e.FirstName.Contains(term) || e.LastName.Contains(term) || (e.MiddleName != null && e.MiddleName.Contains(term)))
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email!
            })
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ThenBy(e => e.MiddleName)
            .ThenBy(e => e.Email)
            .ToListAsync();
    }

    public Task<List<EmployeeForSelectDto>> GetEmployeesForSelectAsync(string term)
    {
        return _userManager.Users
            .AsNoTracking()
            .Where(e => e.FirstName.Contains(term) || e.LastName.Contains(term) || (e.MiddleName != null && e.MiddleName.Contains(term)))
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
        return _userManager.FindByIdAsync(id.ToString());
    }

    public Task<List<Employee>> FindEmployeesByIdAsync(int[] ids)
    {
        return _userManager.Users
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public Task<IdentityResult> CreateAsyncAndSaveChanges(Employee entity, string password)
    {
        return _userManager.CreateAsync(entity, password);
    }

    public Task<IdentityResult> DeleteAndSaveChanges(Employee entity)
    {
        return _userManager.DeleteAsync(entity);
    }

    public async Task<IdentityResult> UpdateAndSaveChanges(EmployeeUpdateRequest dto)
    {
        var employee = await _userManager.FindByIdAsync(dto.Id.ToString()) ?? throw new NotFoundException("Employee not found");

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.MiddleName = dto.MiddleName;

        return await _userManager.UpdateAsync(employee);
    }
}
