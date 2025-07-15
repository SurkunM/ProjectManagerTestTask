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
    private readonly RemoveEmployeeFromProjectHandler _removeEmployeeFromProjectHandler;

    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger,
        CreateProjectHandler createProjectHandler, GetProjectHandler getProjectHandler,
        UpdateProjectHandler updateProjectHandler, DeleteProjectHandler deleteProjectHandler,
        RemoveEmployeeFromProjectHandler removeEmployeeFromProjectHandler, AddEmployeeToProjectHandler addEmployeeToProjectHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _createProjectHandler = createProjectHandler ?? throw new ArgumentNullException(nameof(createProjectHandler));
        _getProjectHandler = getProjectHandler ?? throw new ArgumentNullException(nameof(getProjectHandler));
        _updateProjectHandler = updateProjectHandler ?? throw new ArgumentNullException(nameof(updateProjectHandler));
        _deleteProjectHandler = deleteProjectHandler ?? throw new ArgumentNullException(nameof(deleteProjectHandler));

        _addEmployeeToProjectHandler = addEmployeeToProjectHandler ?? throw new ArgumentNullException(nameof(addEmployeeToProjectHandler));
        _removeEmployeeFromProjectHandler = removeEmployeeFromProjectHandler ?? throw new ArgumentNullException(nameof(removeEmployeeFromProjectHandler));
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
            var success = await _createProjectHandler.HandleAsync(projectDto);

            if (!success)
            {
                _logger.LogError("Project Manager not found to create the project.(ID: {ProjectManagerId})", projectDto.ProjectManagerId);

                return NotFound("Project Manager not found or already deleted");
            }

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create project. Payload: {ProjectDto}.", projectDto);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpPut]
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
            var success = await _updateProjectHandler.HandleAsync(projectDto);

            if (!success)
            {
                _logger.LogError("Project Manager not found to update the project. (ID: {ProjectManagerId})", projectDto.ProjectManagerId);

                return NotFound("Project Manager not found or already deleted");
            }

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
        if (id <= 0)
        {
            _logger.LogError("Invalid project ID provided for deletion: {ProjectId}", id);

            return BadRequest("Valid project ID must be provided.");
        }

        try
        {
            var success = await _deleteProjectHandler.HandleAsync(id);

            if (!success)
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

    [HttpPost]
    public async Task<IActionResult> AddEmployeeToProject([FromBody] int projectId, int employeeId)
    {
        if (projectId <= 0 || employeeId <= 0)
        {
            _logger.LogWarning("Invalid IDs provided - Project: {ProjectId}, Employee: {EmployeeId}", projectId, employeeId);

            return BadRequest("Project ID and Employee ID must be positive integers");
        }

        try
        {
            var success = await _addEmployeeToProjectHandler.HandleAsync(projectId, employeeId);

            if (!success)
            {
                _logger.LogError("Employee {EmployeeId} already exists in project {ProjectId}", employeeId, projectId);

                return Conflict("Employee with ID is already assigned to this project");
            }

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add employee {EmployeeId} to project {ProjectId}.", employeeId, projectId);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveEmployeeFromProject([FromBody] int projectId, int employeeId)
    {
        if (projectId <= 0 || employeeId <= 0)
        {
            _logger.LogWarning("Invalid IDs provided - Project: {ProjectId}, Employee: {EmployeeId}", projectId, employeeId);

            return BadRequest("Project ID and Employee ID must be positive integers");
        }

        try
        {
            var success = await _removeEmployeeFromProjectHandler.HandleAsync(projectId, employeeId);

            if (!success)
            {
                _logger.LogError("ProjectEmployee not found. ProjectId: {ProjectId}, EmployeeId: {EmployeeId}", projectId, employeeId);

                return NotFound("ProjectEmployee not found or already deleted");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove employee {EmployeeId} from project {ProjectId}", employeeId, projectId);

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}
