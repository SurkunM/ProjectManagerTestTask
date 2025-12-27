using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.MappingExtensions;
using ProjectDataManager.Model;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectDataManager.BusinessLogic.IdentityHandlers;

public class EmployeeAuthenticationHandler
{
    private readonly UserManager<Employee> _userManager;

    private readonly SignInManager<Employee> _signInManager;

    private readonly IJwtGenerationService _jwtGenerationService;

    private readonly IJwtBlacklistService _jwtBlacklistService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeAuthenticationHandler(UserManager<Employee> userManager, SignInManager<Employee> signInManager,
        IJwtGenerationService jwtGenerationService, IJwtBlacklistService blacklistService, IHttpContextAccessor httpContextAccessor)
    {
        _jwtGenerationService = jwtGenerationService ?? throw new ArgumentNullException(nameof(jwtGenerationService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _jwtBlacklistService = blacklistService ?? throw new ArgumentNullException(nameof(blacklistService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<EmployeeLoginResponse> LoginHandleAsync(EmployeeLoginRequest loginRequest)
    {
        var employee = await _userManager.FindByNameAsync(loginRequest.UserName) ?? throw new NotFoundException("Employee not found");

        var success = await _userManager.CheckPasswordAsync(employee, loginRequest.Password);

        if (!success)
        {
            throw new InvalidCredentialsException("Invalid email or password");
        }

        var token = await _jwtGenerationService.GenerateTokenAsync(employee);

        var roles = await _userManager.GetRolesAsync(employee);

        return new EmployeeLoginResponse
        {
            Token = token,
            UserData = employee.ToUserDataResponse(roles)
        };
    }

    public void LogoutHandle()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);

        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var expValue = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

        var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expValue)).DateTime;

        _jwtBlacklistService.RemoveToken(jti, expDate);
    }
}
