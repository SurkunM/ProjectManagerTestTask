using Microsoft.AspNetCore.Identity;
using ProjectDataManager.Contracts.Dto.Requests;
using ProjectDataManager.Contracts.Dto.Responses;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Model;

namespace ProjectDataManager.BusinessLogic.IdentityHandlers;

public class EmployeeAuthenticationHandler
{
    private readonly UserManager<Employee> _userManager;

    private readonly SignInManager<Employee> _signInManager;

    private readonly IJwtGenerationService _jwtGenerationService;

    public EmployeeAuthenticationHandler(UserManager<Employee> userManager, SignInManager<Employee> signInManager, IJwtGenerationService jwtGenerationService)
    {
        _jwtGenerationService = jwtGenerationService ?? throw new ArgumentNullException(nameof(jwtGenerationService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<EmployeeLoginResponse> LoginHandleAsync(EmployeeLoginRequest loginRequest)
    {
        var employee = await _userManager.FindByEmailAsync(loginRequest.Email) ?? throw new NotFoundException("Employee not found");

        var success = await _userManager.CheckPasswordAsync(employee, loginRequest.Password);

        if (!success)
        {
            throw new InvalidCredentialsException("Invalid email or password");
        }

        var token = await _jwtGenerationService.GenerateTokenAsync(employee);

        var roles = await _userManager.GetRolesAsync(employee);

        var responseData = new ResponseData//Перенести тот метод в репозиторий Employee
        {
            UserId = employee.Id,
            UserName = $"{employee.LastName} {employee.FirstName[0]}. " +
                $"{(employee.MiddleName is null ? " " : employee.MiddleName[0].ToString() + ".")}",
            Roles = roles
        };

        return new EmployeeLoginResponse
        {
            Token = token,
            UserData = responseData
        };
    }

    public void LogoutHandle()
    {
        _signInManager.SignOutAsync();
    }
}
