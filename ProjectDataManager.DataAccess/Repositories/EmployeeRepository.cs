using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.DataAccess.Repositories.BaseAbstractions;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess.Repositories;

public class EmployeeRepository : BaseEfRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ProjectDataManagerDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Project?> FindEmployeeByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
