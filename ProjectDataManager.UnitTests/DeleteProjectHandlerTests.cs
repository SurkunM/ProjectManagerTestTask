using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.UnitTests;

public class DeleteProjectHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly DeleteProjectHandler _deleteProjectHandler;

    private readonly Mock<ILogger<DeleteProjectHandler>> _loggerMock;

    public DeleteProjectHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteProjectHandler>>();

        _deleteProjectHandler = new DeleteProjectHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        int id = 1;
        var project = Mock.Of<Project>(e => e.Id == id);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(id))
            .ReturnsAsync(project);

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        //var result = await _deleteProjectHandler.HandleAsync(id);

        //Assert.True(result);

        projectsRepositoryMock.Verify(r => r.Delete(project), Times.Once);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);

        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindProjectByIdAsync_ReturnNull()
    {
        int id = 1;
        var project = Mock.Of<Project>(e => e.Id == id);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(id))
            .ReturnsAsync(default(Project));

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        //var result = await _deleteProjectHandler.HandleAsync(id);

        //Assert.False(result);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Once);

        projectsRepositoryMock.Verify(r => r.Delete(project), Times.Never);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_ThrowsDbUpdateException()
    {
        int id = 1;
        var project = Mock.Of<Project>(e => e.Id == id);

        var projectsRepositoryMock = new Mock<IProjectsRepository>();

        projectsRepositoryMock
            .Setup(r => r.FindProjectByIdAsync(id))
            .ReturnsAsync(project);

        projectsRepositoryMock
            .Setup(r => r.Delete(project)).Throws(new DbUpdateException());

        _uowMock
            .Setup(uow => uow.GetRepository<IProjectsRepository>())
            .Returns(projectsRepositoryMock.Object);

        await Assert.ThrowsAsync<DbUpdateException>(() => _deleteProjectHandler.HandleAsync(id));

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Once);

        _uowMock.Verify(uow => uow.SaveAsync(), Times.Never);
    }
}
