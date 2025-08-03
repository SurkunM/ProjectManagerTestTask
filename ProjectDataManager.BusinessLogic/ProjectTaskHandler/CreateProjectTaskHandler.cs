using ProjectDataManager.Contracts.Dto.ProjectTaskDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Contracts.MappingExtensions;

namespace ProjectDataManager.BusinessLogic.ProjectTaskHandler;

public class CreateProjectTaskHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectTaskHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> HandleAsync(ProjectTaskCreateUpdateDto requestDto)
    {
        var projectTasksRepository = _unitOfWork.GetRepository<IProjectTasksRepository>();
        var projectRepository = _unitOfWork.GetRepository<IProjectsRepository>();
        var employeeRepository = _unitOfWork.GetRepository<IEmployeesRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectRepository.FindProjectByIdAsync(requestDto.ProjectId);
            var author = await employeeRepository.FindEmployeeByIdAsync(requestDto.AuthorId);
            var executor = await employeeRepository.FindEmployeeByIdAsync(requestDto.ExecutorId);

            if (project is null || executor is null || author is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            await projectTasksRepository.CreateAsync(requestDto.ToModel(project, executor, author));

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
