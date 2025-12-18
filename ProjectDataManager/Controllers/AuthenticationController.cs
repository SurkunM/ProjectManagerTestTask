using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.IdentityHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;

namespace ProjectDataManager.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly EmployeeAuthenticationHandler _loginHandler;

    public AuthenticationController(EmployeeAuthenticationHandler loginHandler)
    {
        _loginHandler = loginHandler ?? throw new ArgumentNullException(nameof(loginHandler));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<EmployeeLoginResponse> Login(EmployeeLoginRequest loginRequest)
    {
        var result = await _loginHandler.LoginHandleAsync(loginRequest);

        return result;
    }

    [HttpPost]
    [Authorize]
    public IActionResult Logout()
    {
        _loginHandler.LogoutHandle();

        return Ok();
    }
}
