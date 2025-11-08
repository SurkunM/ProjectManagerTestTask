using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.ProjectTaskHandler;
using ProjectDataManager.Contracts.Dto.ProjectTaskDto;

namespace ProjectDataManager.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectTaskController : ControllerBase
{
    private readonly CreateProjectTaskHandler _createProjectTaskHandler;

    private readonly GetProjectTasksHandler _getProjectTasksHandler;

    private readonly UpdateProjectTaskHandler _updateProjectTaskHandler;

    private readonly DeleteProjectTaskHandler _deleteProjectTaskHandler;

    private readonly ILogger<EmployeeController> _logger;

    public ProjectTaskController(ILogger<EmployeeController> logger,
        CreateProjectTaskHandler createProjectTaskHandler, GetProjectTasksHandler getProjectTasksHandler,
        UpdateProjectTaskHandler updateProjectTaskHandler, DeleteProjectTaskHandler deleteProjectTaskHandler)
    {
        _createProjectTaskHandler = createProjectTaskHandler ?? throw new ArgumentNullException(nameof(createProjectTaskHandler));
        _updateProjectTaskHandler = updateProjectTaskHandler ?? throw new ArgumentNullException(nameof(updateProjectTaskHandler));
        _getProjectTasksHandler = getProjectTasksHandler ?? throw new ArgumentNullException(nameof(getProjectTasksHandler));
        _deleteProjectTaskHandler = deleteProjectTaskHandler ?? throw new ArgumentNullException(nameof(deleteProjectTaskHandler));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(string term)
    {
        var tasks = await _getProjectTasksHandler.HandleAsync(term);

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(ProjectTaskCreateUpdateDto taskCreateUpdateDto)
    {
        if (taskCreateUpdateDto is null)
        {
            _logger.LogError("Create failed: Task data payload is null.");

            return BadRequest("Task data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid task create data. Validation errors: {ValidationErrors}, Payload: {taskData}", ModelState, taskCreateUpdateDto);

            return UnprocessableEntity(ModelState);
        }

        await _createProjectTaskHandler.HandleAsync(taskCreateUpdateDto);

        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask(ProjectTaskCreateUpdateDto taskCreateUpdateDto)
    {
        if (taskCreateUpdateDto is null)
        {
            _logger.LogError("Update failed: Task data payload is null.");

            return BadRequest("Task data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid task update data. Validation errors: {ValidationErrors}, Payload: {taskData}", ModelState, taskCreateUpdateDto);

            return UnprocessableEntity(ModelState);
        }

        await _updateProjectTaskHandler.HandleAsync(taskCreateUpdateDto);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
        if (id <= 0)
        {
            _logger.LogError("Invalid task ID provided for deletion: {id}", id);

            return BadRequest("Valid task ID must be provided.");
        }

        await _deleteProjectTaskHandler.HandleAsync(id);

        return NoContent();
    }
}
