using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Dto;

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
    public async Task<ActionResult<EmployeeDto>> GetEmployee([FromQuery] EmployeeDto queryParameters)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! При запросе на получение сотрудников переданы не корректные параметры страницы. ");

            return BadRequest(ModelState);
        }

        try
        {
            var contacts = await _getEmployeeHandler.HandleAsync();

            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Запрос на получение сотрудников не выполнен.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto)
    {
        if (employeeDto is null)
        {
            _logger.LogError("Ошибка! Объект employeeDto пуст.");

            return BadRequest("Объект \"Новый сотрудник\" пуст.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! Переданы не корректные данные для создания сотрудника. {EmployeeDto}", employeeDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            var isCreated = await _createEmployeeHandler.HandleAsync();

            if (!isCreated)
            {
                return BadRequest("");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Сотрудник не создан.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(EmployeeDto employeeDto)
    {
        if (employeeDto is null)
        {
            _logger.LogError("Ошибка! Объект EmployeeDto пуст.");

            return BadRequest("Объект EmployeeDto пуст.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! Не корректно заполнены поля для изменения сотрудника. {EmployeeDto}", employeeDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            var isUpdated = await _updateEmployeeHandler.HandleAsync();

            if (!isUpdated)
            {
                return BadRequest("");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Сотрудник не изменен.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee([FromBody] int id)
    {
        if (id < 0)
        {
            _logger.LogError("Передано значение id меньше нуля. id={id}", id);

            return BadRequest("Передано некорректное значение.");
        }

        try
        {
            var isDeleted = await _deleteEmployeeHandler.HandleAsync();

            if (!isDeleted)
            {
                _logger.LogError("Ошибка! Сотрудник для удаления не найден. id={id}", id);

                return BadRequest("Сотрудник для удаления не найден.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Удаление сотрудника не выполнено. id={id}", id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }
}
