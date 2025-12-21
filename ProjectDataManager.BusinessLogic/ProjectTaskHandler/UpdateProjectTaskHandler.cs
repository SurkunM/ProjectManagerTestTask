using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class UpdateProjectTaskHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<UpdateProjectTaskHandler> _logger;

    public UpdateProjectTaskHandler(IUnitOfWork unitOfWork, ILogger<UpdateProjectTaskHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }

    public async Task HandleAsync(ProjectTaskCreateUpdateDto requestDto)
    {
        var projectTasksRepository = _unitOfWork.GetRepository<IProjectTasksRepository>();
        var projectRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeeService = _unitOfWork.GetService<IEmployeeService>();

        try
        {
            var project = await projectRepository.FindProjectByIdAsync(requestDto.ProjectId) ?? throw new NotFoundException("Project not found");
            var author = await employeeService.FindEmployeeByIdAsync(requestDto.AuthorId) ?? throw new NotFoundException("Author not found");
            var executor = await employeeService.FindEmployeeByIdAsync(requestDto.ExecutorId) ?? throw new NotFoundException("Executor not found");

            projectTasksRepository.Update(requestDto.ToModel(project, executor, author));

            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction rolled back");

            throw;
        }
    }
}
