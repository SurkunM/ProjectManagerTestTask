using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto.Requests;
using ProjectDataManager.Contracts.Exceptions;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;

namespace ProjectDataManager.UnitTests;

public class UpdateEmployeeHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly UpdateEmployeeHandler _updateEmployeeHandler;

    public UpdateEmployeeHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();

        _updateEmployeeHandler = new UpdateEmployeeHandler(_uowMock.Object);
    }

    [Fact]
    public async Task Should_SuccessfullyProcess_UpdateAndSaveChanges_Return_Success()
    {
        var employeesServiceMock = new Mock<IEmployeeService>();
        employeesServiceMock
            .Setup(es => es.UpdateAndSaveChanges(It.IsAny<EmployeeUpdateRequest>()))
            .ReturnsAsync(IdentityResult.Success);

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        await _updateEmployeeHandler.HandleAsync(Mock.Of<EmployeeUpdateRequest>());

        employeesServiceMock.Verify(r => r.UpdateAndSaveChanges(It.IsAny<EmployeeUpdateRequest>()), Times.Once);
    }

    [Fact]
    public async Task Should_OperationFailedException_When_UpdateAndSaveChanges_Return_Failed()
    {
        var employeesServiceMock = new Mock<IEmployeeService>();
        employeesServiceMock
            .Setup(es => es.UpdateAndSaveChanges(It.IsAny<EmployeeUpdateRequest>()))
            .ReturnsAsync(IdentityResult.Failed());

        _uowMock
            .Setup(uow => uow.GetService<IEmployeeService>())
            .Returns(employeesServiceMock.Object);

        await Assert.ThrowsAsync<OperationFailedException>(() => _updateEmployeeHandler.HandleAsync(Mock.Of<EmployeeUpdateRequest>()));

        employeesServiceMock.Verify(r => r.UpdateAndSaveChanges(It.IsAny<EmployeeUpdateRequest>()), Times.Once);
    }
}
