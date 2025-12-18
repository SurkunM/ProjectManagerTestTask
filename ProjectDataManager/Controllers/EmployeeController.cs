using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Responses;
using System.Threading.Tasks;

namespace ProjectDataManager.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController : ControllerBase
{
    private readonly CreateEmployeeHandler _createEmployeeHandler;

    private readonly GetEmployeeHandle _getEmployeeHandler;

    private readonly UpdateEmployeeHandler _updateEmployeeHandler;

    private readonly DeleteEmployeeHandler _deleteEmployeeHandler;

    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger,
        CreateEmployeeHandler createEmployeeHandler, GetEmployeeHandle getEmployeeHandler,
        UpdateEmployeeHandler updateEmployeeHandler, DeleteEmployeeHandler deleteEmployeeHandler)
    {
        _createEmployeeHandler = createEmployeeHandler ?? throw new ArgumentNullException(nameof(createEmployeeHandler));
        _updateEmployeeHandler = updateEmployeeHandler ?? throw new ArgumentNullException(nameof(updateEmployeeHandler));
        _getEmployeeHandler = getEmployeeHandler ?? throw new ArgumentNullException(nameof(getEmployeeHandler));
        _deleteEmployeeHandler = deleteEmployeeHandler ?? throw new ArgumentNullException(nameof(deleteEmployeeHandler));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<ActionResult<List<EmployeeResponseDto>>> GetEmployees(string term = "")
    {
        var employee = await _getEmployeeHandler.HandleAsync(term);

        return Ok(employee);
    }

    [HttpGet]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<ActionResult<List<EmployeeForSelectDto>>> GetEmployeesForSelect(string term = "")
    {
        var employee = await _getEmployeeHandler.GetForSelectHandleAsync(term);

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> CreateEmployee(EmployeeCreateUpdateDto employeeDto)
    {
        if (employeeDto is null)
        {
            _logger.LogError("Employee creation failed: Request payload is null.");

            return BadRequest("Employee data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid employee data provided. Validation errors: {ValidationErrors}, Payload: {EmployeeDto}", ModelState, employeeDto);

            return UnprocessableEntity(ModelState);
        }

        await _createEmployeeHandler.HandleAsync(employeeDto);

        return Created();
    }

    [HttpPut]
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> UpdateEmployee(EmployeeCreateUpdateDto requestDto)
    {
        if (requestDto is null)
        {
            _logger.LogError("Update failed: Employee data payload is null.");

            return BadRequest("Employee data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid employee update data. Validation errors: {ValidationErrors}, Payload: {EmployeeDto}", ModelState, requestDto);

            return UnprocessableEntity(ModelState);
        }

        await _updateEmployeeHandler.HandleAsync(requestDto);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> DeleteEmployee([FromBody] int id)
    {
        if (id <= 0)
        {
            _logger.LogError("Invalid employee ID provided for deletion: {EmployeeId}", id);

            return BadRequest("Valid employee ID must be provided.");
        }

        await _deleteEmployeeHandler.HandleAsync(id);

        return NoContent();
    }
}
