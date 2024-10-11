using FluentValidation;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.Test.User;

public class ActivateAccountCommandHandlerTest
{
    protected readonly Mock<IUserService> MockService;
    protected readonly Mock<IValidator<ActivateAccountCommand>> MockValidator;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;

    public ActivateAccountCommandHandlerTest()
    {
        MockService = new Mock<IUserService>();
        MockValidator = new Mock<IValidator<ActivateAccountCommand>>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();


        MockSecurityContextAccessor
             .Setup(x => x.UserId)
             .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ActivateAccountCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldActivateAccount_WhenCodeIsCorrect()
    {
        var command = new ActivateAccountCommand("test@example.com", "valid-code");
        var user = new OnAim.Admin.Domain.Entities.User
        {
            Email = "test@example.com",
            VerificationCode = "valid-code",
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(5),
            IsActive = false,
            IsVerified = false
        };

        MockService.Setup(s => s.ActivateAccount(command.Email, command.Code)).ReturnsAsync(new ApplicationResult { Success = true });
        var result = await MockService.Object.ActivateAccount(command.Email, command.Code);

        Assert.True(result.Success);
        Assert.Equal("Account activated successfully.", result.Data);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenCodeIsIncorrect()
    {
        var command = new ActivateAccountCommand("test@example.com","invalid-code");
        var user = new OnAim.Admin.Domain.Entities.User
        {
            Email = "test@example.com",
            VerificationCode = "valid-code",
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(5),
            IsActive = false,
            IsVerified = false
        };

        MockService.Setup(s => s.ActivateAccount(command.Email, command.Code)).ThrowsAsync(new BadRequestException("Invalid activation code."));

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => MockService.Object.ActivateAccount(command.Email, command.Code));
        Assert.Equal("Invalid activation code.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenCodeHasExpired()
    {
        var command = new ActivateAccountCommand("test@example.com", "valid-code");
        var user = new OnAim.Admin.Domain.Entities.User
        {
            Email = "test@example.com",
            VerificationCode = "valid-code",
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(-5),
            IsActive = false,
            IsVerified = false
        };

        MockService.Setup(s => s.ActivateAccount(command.Email, command.Code)).ThrowsAsync(new BadRequestException("Activation code has expired."));

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => MockService.Object.ActivateAccount(command.Email, command.Code));
        Assert.Equal("Activation code has expired.", exception.Message);
    }
}
