using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using FluentValidation;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.AdminServices.EmailServices;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.Test.User;

public class CreateUserCommandHandlerTests
{
    protected readonly Mock<IUserService> MockService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IValidator<CreateUserCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public CreateUserCommandHandlerTests()
    {
        MockService = new Mock<IUserService>();
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
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890"
        };

       
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
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
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserIsInactive()
    {
        // Arrange
        var inactiveUser = new OnAim.Admin.Domain.Entities.User(
            "Inactive",
            "User",
            "inactive.user@example.com",
            "inactive.user@example.com",
            "hashedPassword",
            "salt",
            "1234567890",
            1, false, false, null, null, null, false);

        var command = new CreateUserCommand
        {
            FirstName = "Inactive",
            LastName = "User",
            Email = "inactive.user@example.com",
            Phone = "1234567890"
        };
    }
}
