using FluentValidation;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;

namespace OnAim.Admin.Test.User;

public class RegistrationCommandHandlerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IDomainValidationService> _mockDomainValidationService;
    private readonly Mock<IValidator<RegistrationCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public RegistrationCommandHandlerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockEmailService = new Mock<IEmailService>();
        _mockDomainValidationService = new Mock<IDomainValidationService>();
        _mockValidator = new Mock<IValidator<RegistrationCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<RegistrationCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldRegisterNewUser_WhenDomainIsAllowed()
    {
        var command = new RegistrationCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123",
            Phone = "1234567890"
        };

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(true);

        //////////////////////////////
        ///
        var handler = new RegistrationCommandHandler(_mockUserService.Object, _mockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.True(result.Success);
        _mockUserService.Verify(service => service.Registration(
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<DateTime>())
        , Times.Once);

        _mockEmailService.Verify(emailService => emailService.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenDomainIsNotAllowed()
    {
        var command = new RegistrationCommand
        {
            Email = "john.doe@forbidden.com"
        };

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(false);

        var handler = new RegistrationCommandHandler(_mockUserService.Object, _mockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockUserService.Verify(service => service.Registration(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<DateTime>())
        , Times.Once);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenUserAlreadyExistsAndActive()
    {
        var existingUser = new OnAim.Admin.Domain.Entities.User(
            "Jane",
            "Doe",
            "jane.doe@example.com",
            "jane.doe@example.com",
            "hashedPassword",
            "salt",
            "1234567890",
            1,
            false,
            false,
            "",
            VerificationPurpose.AccountActivation,
            new DateTime(),
            false);

        var command = new RegistrationCommand
        {
            Email = "jane.doe@example.com"
        };

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(true);

        ////////////////////////////
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenExistingUserIsInactive()
    {
        var existingUser = new OnAim.Admin.Domain.Entities.User(
            "Inactive",
            "User",
            "inactive.user@example.com",
            "inactive.user@example.com",
            "hashedPassword",
            "salt",
            "1234567890",
            1,
            false,
            false,
            "",
            VerificationPurpose.AccountActivation,
            new DateTime(),
            false)
        { IsActive = false };

        var command = new RegistrationCommand
        {
            Email = "inactive.user@example.com"
        };

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(true);

       /////////////////////////////////
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenExistingUserIsDeleted()
    {
        var deletedUser = new OnAim.Admin.Domain.Entities.User(
            "Deleted",
            "User",
            "deleted.user@example.com",
            "deleted.user@example.com",
            "hashedPassword",
            "salt",
            "1234567890",
            1,
            true,
            false,
            "",
            VerificationPurpose.AccountActivation,
            DateTime.UtcNow,
            false)
        { IsDeleted = true };

        var command = new RegistrationCommand
        {
            Email = "deleted.user@example.com",
            Password = "12345",
        };

        var deletedList = new List<OnAim.Admin.Domain.Entities.User> { deletedUser };

       /////////////////////////////
    }
}