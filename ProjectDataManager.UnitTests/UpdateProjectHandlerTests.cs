using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.Dto.ProjectDto;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.UnitTests;

public class UpdateProjectHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly UpdateProjectHandler _updateProjectHandler;

    private readonly Mock<ILogger<UpdateProjectHandler>> _loggerMock;

    public UpdateProjectHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateProjectHandler>>();

        _updateProjectHandler = new UpdateProjectHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        int id = 1;
        var manager = Mock.Of<Employee>(e => e.Id == id);

        var dto = Mock.Of<ProjectCreateUpdateDto>(pDto => pDto.ProjectManagerId == id);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(r => r.FindEmployeeByIdAsync(dto.ProjectManagerId))
            .ReturnsAsync(manager);

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        await _updateProjectHandler.HandleAsync(dto);

        employeesServiceMock.Verify(r => r.FindEmployeeByIdAsync(id), Times.Once);
        projectsRepositoryMock.Verify(r => r.Update(It.IsAny<Project>()), Times.Once);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);

        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindEmployeeByIdAsync_ReturnNull()
    {
        var dto = Mock.Of<ProjectCreateUpdateDto>(pDto => pDto.ProjectManagerId == 1);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(r => r.FindEmployeeByIdAsync(dto.ProjectManagerId))
            .ReturnsAsync(default(Employee));

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateProjectHandler.HandleAsync(dto));

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Once);

        projectsRepositoryMock.Verify(r => r.Update(It.IsAny<Project>()), Times.Never);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_ThrowsDbUpdateException()
    {
        var manager = Mock.Of<Employee>(e => e.Id == 1);
        var dto = Mock.Of<ProjectCreateUpdateDto>(pDto => pDto.ProjectManagerId == 1);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(r => r.FindEmployeeByIdAsync(dto.ProjectManagerId))
            .ReturnsAsync(manager);

        projectsRepositoryMock
            .Setup(r => r.Update(It.IsAny<Project>())).Throws(new DbUpdateException());

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        await Assert.ThrowsAsync<DbUpdateException>(() => _updateProjectHandler.HandleAsync(dto));

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Once);

        _uowMock.Verify(uow => uow.SaveAsync(), Times.Never);
    }
}
