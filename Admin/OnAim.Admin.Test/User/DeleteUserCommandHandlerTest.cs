using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Test.User;

public class DeleteUserCommandHandlerTests
{
    protected readonly Mock<IUserService> MockService;
    protected readonly Mock<IValidator<DeleteUserCommand>> MockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;


    public DeleteUserCommandHandlerTests()
    {
        MockService = new Mock<IUserService>();
        MockValidator = new Mock<IValidator<DeleteUserCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<DeleteUserCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    //[Fact]
    //public async Task Handle_ShouldDeleteUsers_WhenUsersExist()
    //{
    //    var userIds = new List<int> { 1, 2 };
    //    var users = new List<OnAim.Admin.Domain.Entities.User>
    //    {
    //         new OnAim.Admin.Domain.Entities.User(
    //        "John",
    //        "Doe",
    //        "johndoe",
    //        "john.doe@example.com",
    //        "hashedpassword",
    //        "salt",
    //        "1234567890",
    //        createdBy: null,
    //        isVerified: true,
    //        isActive: true,
    //        verificationCode: null,
    //        verificationPurpose: null,
    //        verificationCodeExpiration: null,
    //        isSuperAdmin: false
    //    ),
    //    new OnAim.Admin.Domain.Entities.User(
    //        "Jane",
    //        "Doe",
    //        "janedoe",
    //        "jane.doe@example.com",
    //        "hashedpassword",
    //        "salt",
    //        "0987654321",
    //        createdBy: null,
    //        isVerified: true,
    //        isActive: true,
    //        verificationCode: null,
    //        verificationPurpose: null,
    //        verificationCodeExpiration: null,
    //        isSuperAdmin: false
    //        )
    //    }.AsQueryable().BuildMock();
    //    var command = new DeleteUserCommand(userIds);
    //    var filter = new UserFilter();

    //    MockService
    //       .Setup(service => service.GetAll(filter))
    //       .ReturnsAsync(new ApplicationResult { Data = users.AsQueryable() });//??????

    //    MockService
    //       .Setup(service => service.Delete(It.IsAny<List<int>>()))
    //       .ReturnsAsync(new ApplicationResult { Success = true })
    //       .Verifiable();

    //    var handler = new DeleteUserCommandHandler(MockService.Object, MockValidator.Object);

    //    var result = await handler.Handle(command, CancellationToken.None);

    //    Assert.True(result.Success);
    //    MockService.Verify(service => service.Delete(It.IsAny<List<int>>()), Times.Once);
    //}

    //[Fact]
    //public async Task Handle_ShouldThrowNotFoundException_WhenNoUsersFound()
    //{
    //    var userIds = new List<int> { 3, 4 };
    //    var emptyUsers = new List<OnAim.Admin.Domain.Entities.User>().AsQueryable().BuildMock();
    //    var command = new DeleteUserCommand(userIds);

    //    var filter = new UserFilter( );

    //    MockService
    //       .Setup(service => service.GetAll(filter))
    //       .ReturnsAsync(new ApplicationResult { Data = emptyUsers.AsQueryable() });//??????

    //    MockService
    //      .Setup(service => service.Delete(It.IsAny<List<int>>()))
    //      .ReturnsAsync(new ApplicationResult { Success = true })
    //      .Verifiable();

    //    var handler = new DeleteUserCommandHandler(MockService.Object, MockValidator.Object);

    //    var result = await handler.Handle(command, CancellationToken.None);

    //    Assert.True(result.Success);
    //    MockService.Verify(service => service.Delete(It.IsAny<List<int>>()), Times.Once);

    //}
}
