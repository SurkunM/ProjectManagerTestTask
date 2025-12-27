using Microsoft.Extensions.Caching.Memory;
using ProjectDataManager.Contracts.IServices;

namespace ProjectDataManager.BusinessLogic.Services;

public class JwtBlacklistService : IJwtBlacklistService
{
    private readonly IMemoryCache _memoryCache;

    public JwtBlacklistService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public bool IsTokenRevoked(string jti)
    {
        return _memoryCache.TryGetValue(jti, out _);
    }

    public void RemoveToken(string jti, DateTime expirationDate)
    {
        var expiry = expirationDate - DateTime.UtcNow;

        if (expiry > TimeSpan.Zero)
        {
            _memoryCache.Set(jti, true, expirationDate);
        }
    }
}
