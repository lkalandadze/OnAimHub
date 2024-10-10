using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using MockQueryable;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.Test.User;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IValidator<CreateUserCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public CreateUserCommandHandlerTests()
    {
        _mockEmailService = new Mock<IEmailService>();
        _mockValidator = new Mock<IValidator<CreateUserCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateUserCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenUserDoesNotExist()
    {
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890"
        };

        var emptyUserList = new List<OnAim.Admin.Domain.Entities.User>().AsQueryable().BuildMock();

       
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserAlreadyExists()
    {
        var existingUser = new OnAim.Admin.Domain.Entities.User(
            "Jane",
            "Doe",
            "jane.doe@example.com",
            "jane.doe@example.com",
            "hashedPassword",
            "salt",
            "1234567890",
            1, true, true, null, null, null, false);

        var command = new CreateUserCommand
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Phone = "1234567890"
        };
        var existingList = new List<OnAim.Admin.Domain.Entities.User> { existingUser }.AsQueryable();

       

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
    }
}
