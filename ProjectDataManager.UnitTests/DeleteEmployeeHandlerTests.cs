using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
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

        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(es => es.FindEmployeeByIdAsync(id))
            .ReturnsAsync(employee);

        employeesServiceMock
            .Setup(es => es.DeleteAndSaveChanges(It.IsAny<Employee>()))
            .ReturnsAsync(IdentityResult.Success);

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        await _deleteEmployeeHandler.HandleAsync(id);

        employeesServiceMock.Verify(r => r.FindEmployeeByIdAsync(id), Times.Once);
        employeesServiceMock.Verify(r => r.DeleteAndSaveChanges(employee), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowNotFoundException_When_FindEmployeeByIdAsync_ReturnNull()
    {
        int id = 1;

        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(r => r.FindEmployeeByIdAsync(id))
            .ReturnsAsync(default(Employee));

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => _deleteEmployeeHandler.HandleAsync(id));

        employeesServiceMock.Verify(es => es.FindEmployeeByIdAsync(id), Times.Once);
        employeesServiceMock.Verify(es => es.DeleteAndSaveChanges(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowsOperationFailedException()
    {
        int id = 1;
        var employee = Mock.Of<Employee>(e => e.Id == id);

        var employeesServiceMock = new Mock<IEmployeeService>();

        employeesServiceMock
            .Setup(r => r.FindEmployeeByIdAsync(id))
            .ReturnsAsync(employee);

        employeesServiceMock
            .Setup(r => r.DeleteAndSaveChanges(employee))
            .ReturnsAsync(IdentityResult.Failed());

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        await Assert.ThrowsAsync<OperationFailedException>(() => _deleteEmployeeHandler.HandleAsync(id));

        employeesServiceMock.Verify(es => es.FindEmployeeByIdAsync(id), Times.Once);
        employeesServiceMock.Verify(es => es.DeleteAndSaveChanges(It.IsAny<Employee>()), Times.Once);
    }
}
