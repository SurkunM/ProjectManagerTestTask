using Microsoft.AspNetCore.Mvc;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Dto.QueryParameters;

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
    public async Task<ActionResult<ProjectCreateUpdateDto>> GetProject([FromQuery] GetProjectsQueryParameters queryParameters)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid parameters provided for projects list request. Validation errors: {ValidationErrors}", ModelState);

            return BadRequest(ModelState);
        }

        try
        {
            var projects = await _getProjectHandler.HandleAsync(queryParameters);

            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve project list.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectCreateUpdateDto projectDto)
    {
        if (projectDto is null)
        {
            _logger.LogError("Project creation failed: Request payload is null.");

            return BadRequest("Project data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid project data provided. Validation errors: {ValidationErrors}, Payload: {ProjectDto}", ModelState, projectDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            await _createProjectHandler.HandleAsync(projectDto);

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create project. Payload: {ProjectDto}.", projectDto);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProject(ProjectCreateUpdateDto projectDto)
    {
        if (projectDto is null)
        {
            _logger.LogError("Update failed: Project data payload is null.");

            return BadRequest("Project data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid project update data. Validation errors: {ValidationErrors}, Payload: {ProjectId}", ModelState, projectDto);

            return UnprocessableEntity(ModelState);
        }

        try
        {
            await _updateProjectHandler.HandleAsync(projectDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update project (ID: {ProjectId}).", projectDto.Id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject([FromBody] int id)
    {
        if (id < 0)
        {
            _logger.LogError("Invalid project ID provided for deletion: {ProjectId}", id);

            return BadRequest("Valid project ID must be provided.");
        }

        try
        {
            var isDeleted = await _deleteProjectHandler.HandleAsync(id);

            if (!isDeleted)
            {
                _logger.LogError("Project not found for deletion (ID: {ProjectId})", id);

                return NotFound("Project not found or already deleted");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete project (ID: {ProjectId})", id);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}
