using Moq;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.Contracts.Dto.EmployeeDto;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.Model;

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
    public async Task Should_SuccessfullyProcess_AllStepsAndSaveChanges()
    {
        var employeesRepositoryMock = new Mock<IEmployeesRepository>();

        _uowMock
            .Setup(uow => uow.GetRepository<IEmployeesRepository>())
            .Returns(employeesRepositoryMock.Object);

        await _updateEmployeeHandler.HandleAsync(Mock.Of<EmployeeCreateUpdateDto>());

        employeesRepositoryMock.Verify(r => r.Update(It.IsAny<Employee>()), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}
