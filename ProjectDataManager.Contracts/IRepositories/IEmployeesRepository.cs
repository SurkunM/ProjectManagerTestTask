using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IEmployeesRepository : IRepository<Employee>
{
    Task<Employee?> FindEmployeeByIdAsync(int id);

    Task<List<EmployeeResponseDto>> GetEmployeesAsync(string term);

    Task<List<EmployeeForSelectDto>> GetEmployeesForSelectAsync(string term);
}
