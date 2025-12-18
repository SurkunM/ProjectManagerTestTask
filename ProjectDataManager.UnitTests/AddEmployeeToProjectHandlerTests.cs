using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.UnitTests;

public class AddEmployeeToProjectHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly AddEmployeeToProjectHandler _addEmployeeToProjectHandler;

    private readonly Mock<ILogger<AddEmployeeToProjectHandler>> _loggerMock;

    private static readonly int[] _ids = [1, 2];

    private static readonly int _projectId = 1;

    public AddEmployeeToProjectHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<AddEmployeeToProjectHandler>>();

        _addEmployeeToProjectHandler = new AddEmployeeToProjectHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        var project = Mock.Of<Project>();

        var employees = new List<Employee>
        {
            Mock.Of<Employee>(),
            Mock.Of<Employee>()
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(_projectId))
            .ReturnsAsync(project);

        employeesRepositoryMock
            .Setup(r => r.FindEmployeesByIdAsync(_ids))
            .ReturnsAsync(employees);

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

         await _addEmployeeToProjectHandler.HandleAsync(_projectId, _ids);

        //Assert.True();

        employeesRepositoryMock.Verify(r => r.FindEmployeesByIdAsync(_ids), Times.Once);

        projectsRepositoryMock.Verify(r => r.FindProjectByIdAsync(_projectId), Times.Once);
        projectsRepositoryMock.Verify(r => r.AddEmployeesToProject(project, employees), Times.Once);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_ThrowsDbUpdateException()
    {
        var project = Mock.Of<Project>();

        var employees = new List<Employee>
        {
            Mock.Of<Employee>(),
            Mock.Of<Employee>()
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(_projectId))
            .ReturnsAsync(project);

        projectsRepositoryMock
            .Setup(r => r.AddEmployeesToProject(project, employees))
            .Throws(new DbUpdateException());

        employeesRepositoryMock
            .Setup(r => r.FindEmployeesByIdAsync(_ids))
            .ReturnsAsync(employees);

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        await Assert.ThrowsAsync<DbUpdateException>(() => _addEmployeeToProjectHandler.HandleAsync(_projectId, _ids));

        _uowMock.Verify(u => u.SaveAsync(), Times.Never);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindEmployeesByIdAsync_ReturnEmptyList()
    {
        var project = Mock.Of<Project>();

        var employees = new List<Employee>
        {
            Mock.Of<Employee>(),
            Mock.Of<Employee>()
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(_projectId))
            .ReturnsAsync(project);

        employeesRepositoryMock
            .Setup(r => r.FindEmployeesByIdAsync(_ids))
            .ReturnsAsync(new List<Employee>());

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        //var result = await _addEmployeeToProjectHandler.HandleAsync(_projectId, _ids);

       // Assert.False(result);

        _uowMock.Verify(u => u.SaveAsync(), Times.Never);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindProjectByIdAsync_ReturnNull()
    {
        var employees = new List<Employee>
        {
            Mock.Of<Employee>(),
            Mock.Of<Employee>()
        };

        var projectsRepositoryMock = new Mock<IProjectsRepository>();
        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(_projectId))
            .ReturnsAsync(default(Project));

        employeesRepositoryMock
            .Setup(r => r.FindEmployeesByIdAsync(_ids))
            .ReturnsAsync(employees);

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        //var result = await _addEmployeeToProjectHandler.HandleAsync(_projectId, _ids);

        //Assert.False(result);

        _uowMock.Verify(u => u.SaveAsync(), Times.Never);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);
    }
}
