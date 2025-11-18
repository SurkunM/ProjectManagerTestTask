using ProjectDataManager.Model;

namespace ProjectDataManager.Contracts.IServices;

public interface IJwtGenerationService
{
    Task<string> GenerateTokenAsync(Employee employee);
}
