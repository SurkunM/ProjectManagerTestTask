using Microsoft.AspNetCore.Identity;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IServices;

public interface IEmployeeService
{
    Task<Employee?> FindEmployeeByIdAsync(int id);

    Task<List<Employee>> FindEmployeesByIdAsync(int[] ids);

    Task<List<EmployeeResponseDto>> GetEmployeesAsync(string term);

    Task<List<EmployeeForSelectDto>> GetEmployeesForSelectAsync(string term);

    Task<IdentityResult> CreateAsyncAndSaveChanges(Employee entity);

    Task<IdentityResult> UpdateAndSaveChanges(EmployeeCreateUpdateDto dto);

    Task<IdentityResult> DeleteAndSaveChanges(Employee entity);
}
