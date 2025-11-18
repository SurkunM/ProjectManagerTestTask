using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.Settings;
using ProjectDataManager.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectDataManager.BusinessLogic.Services;

public class JwtGenerationService : IJwtGenerationService
{
    private readonly UserManager<Employee> _userManager;

    private readonly JwtSettings _jwtSettings;

    public JwtGenerationService(UserManager<Employee> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> GenerateTokenAsync(Employee employee)
    {
        var claims = await GenerateClaimsAsync(employee);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(_jwtSettings.ExpiryHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<List<Claim>> GenerateClaimsAsync(Employee employee)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new(ClaimTypes.Name, employee.UserName ?? string.Empty),
            new("ManagerId", employee.ManagerId.ToString() ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(employee);

        claims.AddRange(roles
            .Select(r => new Claim(ClaimTypes.Role, r)));

        return claims;
    }
}
