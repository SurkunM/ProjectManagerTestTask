namespace ProjectDataManager.Contracts.IServices;

public interface IJwtBlacklistService
{
    void RemoveToken(string jti, DateTime expirationDate);

    bool IsTokenRevoked(string jti);
}
