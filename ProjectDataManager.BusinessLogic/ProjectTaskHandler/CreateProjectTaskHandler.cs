using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class CreateProjectTaskHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<CreateProjectTaskHandler> _logger;

    public CreateProjectTaskHandler(IUnitOfWork unitOfWork, ILogger<CreateProjectTaskHandler> logger)
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
            _unitOfWork.BeginTransaction();

            var project = await projectRepository.FindProjectByIdAsync(requestDto.ProjectId) ?? throw new NotFoundException("Project not found");
            var author = await employeeService.FindEmployeeByIdAsync(requestDto.AuthorId) ?? throw new NotFoundException("Author not found");
            var executor = await employeeService.FindEmployeeByIdAsync(requestDto.ExecutorId) ?? throw new NotFoundException("Executor not found");

            await projectTasksRepository.CreateAsync(requestDto.ToModel(project, executor, author));

            await _unitOfWork.SaveAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Transaction rolled back");

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
