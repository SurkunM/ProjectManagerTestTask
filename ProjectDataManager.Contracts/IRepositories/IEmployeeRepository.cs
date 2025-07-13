using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IRepositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Project?> FindEmployeeByIdAsync(int id);
}
