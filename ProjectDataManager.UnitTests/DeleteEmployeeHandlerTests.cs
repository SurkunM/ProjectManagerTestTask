using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

namespace ProjectDataManager.UnitTests;

public class DeleteEmployeeHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly DeleteEmployeeHandler _deleteEmployeeHandler;

    private readonly Mock<ILogger<DeleteEmployeeHandler>> _loggerMock;

    public DeleteEmployeeHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteEmployeeHandler>>();

        _deleteEmployeeHandler = new DeleteEmployeeHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        int id = 1;
        var employee = Mock.Of<Employee>(e => e.Id == id);

        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        employeesRepositoryMock
            .Setup(r => r.FindEmployeeByIdAsync(id))
            .ReturnsAsync(employee);

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        var result = await _deleteEmployeeHandler.HandleAsync(id);

        Assert.True(result);

        employeesRepositoryMock.Verify(r => r.Delete(employee), Times.Once);

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);

        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_FindEmployeeByIdAsync_ReturnNull()
    {
        int id = 1;

        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        employeesRepositoryMock
            .Setup(r => r.FindEmployeeByIdAsync(id))
            .ReturnsAsync(default(Employee));

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        var result = await _deleteEmployeeHandler.HandleAsync(id);

        Assert.False(result);

        _uowMock.Verify(u => u.BeginTransaction(), Times.Once);
        _uowMock.Verify(u => u.RollbackTransaction(), Times.Once);

        employeesRepositoryMock.Verify(r => r.Delete(Mock.Of<Employee>()), Times.Never);
        _uowMock.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_RollbackTransaction_When_ThrowsDbUpdateException()
    {
        int id = 1;
        var employee = Mock.Of<Employee>(e => e.Id == id);

        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        employeesRepositoryMock
            .Setup(r => r.FindEmployeeByIdAsync(id))
            .ReturnsAsync(employee);

        employeesRepositoryMock
            .Setup(r => r.Delete(employee))
            .Throws(new DbUpdateException());

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        await Assert.ThrowsAsync<DbUpdateException>(() => _deleteEmployeeHandler.HandleAsync(id));

        _uowMock.Verify(uow => uow.BeginTransaction(), Times.Once);
        _uowMock.Verify(uow => uow.RollbackTransaction(), Times.Once);

        _uowMock.Verify(uow => uow.SaveAsync(), Times.Never);
    }
}
