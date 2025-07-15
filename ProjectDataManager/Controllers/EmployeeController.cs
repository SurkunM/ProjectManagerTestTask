using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto;

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
    public async Task<ActionResult<List<EmployeeResponseDto>>> GetEmployees(string term)
    {
        try
        {
            var employee = await _getEmployeeHandler.HandleAsync(term);

            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve employee list.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeForSelectDto>>> GetEmployeesForSelect(string term)
    {
        try
        {
            var employee = await _getEmployeeHandler.GetForSelectHandleAsync(term);

            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve employee list.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost]
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

        try
        {
            await _createEmployeeHandler.HandleAsync(employeeDto);

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create employee. Payload: {EmployeeDto}.", employeeDto);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpPut]
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

        try
        {
            await _updateEmployeeHandler.HandleAsync(requestDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update employee (ID: {EmployeeId}).", requestDto.Id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee([FromBody] int id)
    {
        if (id <= 0)
        {
            _logger.LogError("Invalid employee ID provided for deletion: {EmployeeId}", id);

            return BadRequest("Valid employee ID must be provided.");
        }

        try
        {
            var success = await _deleteEmployeeHandler.HandleAsync(id);

            if (!success)
            {
                _logger.LogError("Employee not found for deletion (ID: {EmployeeId})", id);

                return NotFound("Employee not found or already deleted");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete employee (ID: {EmployeeId})", id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}
