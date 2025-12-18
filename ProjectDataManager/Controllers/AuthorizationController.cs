using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.IdentityHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;

namespace ProjectDataManager.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthorizationController : ControllerBase
{
    private readonly EmployeeAuthorizationHandler _accountAuthorizationHandlers;

    public AuthorizationController(EmployeeAuthorizationHandler authorizationHandlers)
    {
        _accountAuthorizationHandlers = authorizationHandlers ?? throw new ArgumentNullException(nameof(authorizationHandlers));
    }

    [HttpPost]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<IActionResult> AccountRegister(EmployeeRegisterRequest employeeRegisterRequest)
    {
        await _accountAuthorizationHandlers.HandleAsync(employeeRegisterRequest);

        return Ok();
    }
}
