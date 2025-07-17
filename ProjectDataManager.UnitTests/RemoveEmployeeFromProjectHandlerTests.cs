using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.UnitTests;

public class RemoveEmployeeFromProjectHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly RemoveEmployeeFromProjectHandler _removeEmployeeFromProjectHandler;

    private readonly Mock<ILogger<RemoveEmployeeFromProjectHandler>> _loggerMock;

    private static readonly int[] _ids = [1, 2];

    private static readonly int _projectId = 1;

    public RemoveEmployeeFromProjectHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<RemoveEmployeeFromProjectHandler>>();

        _removeEmployeeFromProjectHandler = new RemoveEmployeeFromProjectHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        var projectEmployees = new List<ProjectEmployee>
        {
            Mock.Of<ProjectEmployee>(pe =>
                pe.Id == 1 &&

                pe.EmployeeId == _ids[0] &&
                pe.Employee == Mock.Of<Employee>()&&

                pe.ProjectId == _projectId &&
                pe.Project == Mock.Of<Project>()
            ),
            Mock.Of<ProjectEmployee>(pe =>
                pe.Id == 2 &&

                pe.EmployeeId == _ids[1] &&
                pe.Employee == Mock.Of<Employee>()&&

                pe.ProjectId == _projectId &&
                pe.Project == Mock.Of<Project>()
            )
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectEmployeeByIdAsync(_projectId, _ids))
            .ReturnsAsync(projectEmployees);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        var result = await _removeEmployeeFromProjectHandler.HandleAsync(_projectId, _ids);

        Assert.True(result);

        projectsRepositoryMock.Verify(r => r.FindProjectEmployeeByIdAsync(_projectId, _ids), Times.Once);
        projectsRepositoryMock.Verify(r => r.RemoveEmployeesFromProject(projectEmployees), Times.Once);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_ThrowsDbUpdateException()
    {
        var projectEmployees = new List<ProjectEmployee>
        {
            Mock.Of<ProjectEmployee>(pe =>
                pe.Id == 1 &&

                pe.EmployeeId == _ids[0] &&
                pe.Employee == Mock.Of<Employee>()&&

                pe.ProjectId == _projectId &&
                pe.Project == Mock.Of<Project>()
            ),
            Mock.Of<ProjectEmployee>(pe =>
                pe.Id == 2 &&

                pe.EmployeeId == _ids[1] &&
                pe.Employee == Mock.Of<Employee>()&&

                pe.ProjectId == _projectId &&
                pe.Project == Mock.Of<Project>()
            )
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectEmployeeByIdAsync(_projectId, _ids))
            .ReturnsAsync(projectEmployees);
        projectsRepositoryMock
            .Setup(r => r.RemoveEmployeesFromProject(projectEmployees))
            .Throws(new DbUpdateException());

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>()).Returns(projectsRepositoryMock.Object);

        await Assert.ThrowsAsync<DbUpdateException>(() => _removeEmployeeFromProjectHandler.HandleAsync(_projectId, _ids));

        _uowMock.Verify(u => u.SaveAsync(), Times.Never);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindProjectEmployeesByIdAsync_ReturnEmptyList()
    {
        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectEmployeeByIdAsync(_projectId, _ids))
            .ReturnsAsync(new List<ProjectEmployee>());

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        var result = await _removeEmployeeFromProjectHandler.HandleAsync(_projectId, _ids);

        Assert.False(result);

        _uowMock.Verify(u => u.SaveAsync(), Times.Never);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);
    }
}