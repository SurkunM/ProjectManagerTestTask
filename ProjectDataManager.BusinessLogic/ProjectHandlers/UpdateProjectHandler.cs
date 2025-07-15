using Microsoft.Extensions.Logging;
using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class UpdateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<UpdateProjectHandler> _logger;

    public UpdateProjectHandler(IUnitOfWork unitOfWork, ILogger<UpdateProjectHandler> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> HandleAsync(ProjectCreateUpdateDto projectDto)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeeRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var manager = await employeeRepository.FindEmployeeByIdAsync(projectDto.ProjectManagerId);

            if (manager is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            projectsRepository.Update(projectDto.ToModel(manager));

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create project. Transaction rolled back");

            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
