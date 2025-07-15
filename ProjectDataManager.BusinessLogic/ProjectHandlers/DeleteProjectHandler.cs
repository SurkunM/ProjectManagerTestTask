using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.BusinessLogic.ProjectHandlers;

public class DeleteProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(int id)
    {
        var projectsRepository = _unitOfWork.GetRepository<IProjectsRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var project = await projectsRepository.FindProjectByIdAsync(id);

            if (project is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            projectsRepository.Delete(project);

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
