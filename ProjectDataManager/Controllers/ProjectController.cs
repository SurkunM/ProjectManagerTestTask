using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts;

namespace ProjectDataManager.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectController : ControllerBase
{
    private readonly CreateProjectHandler _createProjectHandler;

    private readonly GetProjectHandler _getProjectHandler;

    private readonly UpdateProjectHandler _updateProjectHandler;

    private readonly DeleteProjectHandler _deleteProjectHandler;

    private readonly AddEmployeeToProjectHandler _addEmployeeToProjectHandler;
    private readonly DeleteEmployeeFromProjectHandler _deleteEmployeeFromProjectHandler;

    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger,
        CreateProjectHandler createProjectHandler,
        GetProjectHandler getProjectHandler,
        UpdateProjectHandler updateProjectHandler,
       DeleteProjectHandler deleteProjectHandler,
        DeleteEmployeeFromProjectHandler deleteEmployeeFromProjectHandler, AddEmployeeToProjectHandler addEmployeeToProjectHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _createProjectHandler = createProjectHandler ?? throw new ArgumentNullException(nameof(createProjectHandler));
        _getProjectHandler = getProjectHandler ?? throw new ArgumentNullException(nameof(getProjectHandler));
        _updateProjectHandler = updateProjectHandler ?? throw new ArgumentNullException(nameof(updateProjectHandler));
        _deleteProjectHandler = deleteProjectHandler ?? throw new ArgumentNullException(nameof(deleteProjectHandler));

        _deleteEmployeeFromProjectHandler = deleteEmployeeFromProjectHandler ?? throw new ArgumentNullException(nameof(deleteEmployeeFromProjectHandler));
        _addEmployeeToProjectHandler = addEmployeeToProjectHandler ?? throw new ArgumentNullException(nameof(addEmployeeToProjectHandler));
    }

    [HttpGet]
    public async Task<ActionResult<ProjectDto>> GetProject([FromQuery] ProjectDto queryParameters)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! При запросе на получение проекта переданы не корректные параметры страницы. ");

            return BadRequest(ModelState);
        }

        try
        {
            var contacts = await _getProjectHandler.HandleAsync();

            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Запрос на получение проектов не выполнен.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectDto projectDto)
    {
        if (projectDto is null)
        {
            _logger.LogError("Ошибка! Объект ProjectDto пуст.");

            return BadRequest("Объект \"Новый проект\" пуст.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! Переданы не корректные данные для создания проекта. {ProjectDto}", projectDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            var isCreated = await _createProjectHandler.HandleAsync();

            if (!isCreated)
            {
                return BadRequest("");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Проект не создан.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProject(ProjectDto projectDto)
    {
        if (projectDto is null)
        {
            _logger.LogError("Ошибка! Объект ProjectDto пуст.");

            return BadRequest("Объект ProjectDto пуст.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Ошибка! Не корректно заполнены поля для изменения проекта. {ProjectDto}", projectDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            var isUpdated = await _updateProjectHandler.HandleAsync();

            if (!isUpdated)
            {
                return BadRequest("");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Проект не изменен.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject([FromBody] int id)
    {
        if (id < 0)
        {
            _logger.LogError("Передано значение id меньше нуля. id={id}", id);

            return BadRequest("Передано некорректное значение.");
        }

        try
        {
            var isDeleted = await _deleteProjectHandler.HandleAsync();

            if (!isDeleted)
            {
                _logger.LogError("Ошибка! Проект для удаления не найден. id={id}", id);

                return BadRequest("Проект для удаления не найден.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка! Удаление проекта не выполнено. id={id}", id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
        }
    }
}
