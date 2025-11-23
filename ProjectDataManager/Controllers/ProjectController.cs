using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<ActionResult<ProjectCreateUpdateDto>> GetProjects([FromQuery] GetProjectsQueryParameters queryParameters)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid parameters provided for projects list request. Validation errors: {ValidationErrors}", ModelState);

            return BadRequest(ModelState);
        }

        var projects = await _getProjectHandler.HandleAsync(queryParameters);

        return Ok(projects);
    }

    [HttpPost]
    [Authorize(Roles = "Supervisor,ProjectManager")]
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

        await _createProjectHandler.HandleAsync(projectDto);

        return Created();
    }

    [HttpPut]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<IActionResult> UpdateProject(ProjectCreateUpdateDto projectDto)
    {
        if (projectDto is null)
        {
            _logger.LogError("Update failed: Project data payload is null.");

            return BadRequest("Project data is required.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid project update data. Validation errors: {ValidationErrors}, Payload: {ProjectDto}", ModelState, projectDto);

            return UnprocessableEntity(ModelState);
        }

        await _updateProjectHandler.HandleAsync(projectDto);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<IActionResult> DeleteProject([FromBody] int id)
    {
        if (id <= 0)
        {
            _logger.LogError("Invalid project ID provided for deletion: {ProjectId}", id);

            return BadRequest("Valid project ID must be provided.");
        }

        await _deleteProjectHandler.HandleAsync(id);

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<IActionResult> AddEmployeesToProject(ProjectRemoveAddEmployeesDto addEmployeesDto)
    {
        if (addEmployeesDto.ProjectId <= 0)
        {
            _logger.LogWarning("Invalid ID provided - Project: {ProjectId}", addEmployeesDto.ProjectId);

            return BadRequest("Project ID must be positive integers");
        }

        await _addEmployeeToProjectHandler.HandleAsync(addEmployeesDto.ProjectId, addEmployeesDto.EmployeesId);

        return Created();
    }

    [HttpDelete]
    [Authorize(Roles = "Supervisor,ProjectManager")]
    public async Task<IActionResult> RemoveEmployeesFromProject(ProjectRemoveAddEmployeesDto removeEmployeesDto)
    {
        if (removeEmployeesDto.ProjectId <= 0)
        {
            _logger.LogWarning("Invalid ID provided - Project: {ProjectId}", removeEmployeesDto.ProjectId);

            return BadRequest("Project ID must be positive integers");
        }

        await _removeEmployeeFromProjectHandler.HandleAsync(removeEmployeesDto.ProjectId, removeEmployeesDto.EmployeesId);

        return NoContent();
    }
}
