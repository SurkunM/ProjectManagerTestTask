using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class CreateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<CreateProjectHandler> _logger;

    public CreateProjectHandler(IUnitOfWork unitOfWork, ILogger<CreateProjectHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(ProjectCreateUpdateDto projectDto)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeeRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var manager = await employeeRepository.FindEmployeeByIdAsync(projectDto.ProjectManagerId) ?? throw new NotFoundException("Project Manager not found or already deleted");
            var projectModel = projectDto.ToModel(manager);

            await projectsRepository.CreateAsync(projectModel);

            if (projectDto.ProjectEmployeesId.Length != 0)
            {
                var employees = await employeeRepository.FindEmployeesByIdAsync(projectDto.ProjectEmployeesId);

                if (employees.Count > 0)
                {
                    await projectsRepository.AddEmployeesToProject(projectModel, employees);
                }
            }

            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction rolled back");

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
