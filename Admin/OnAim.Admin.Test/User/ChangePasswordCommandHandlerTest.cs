using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;
using OnAim.Admin.APP.Services.AuthServices;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Test.User;

public class ChangePasswordCommandHandlerTest
{
    private readonly Mock<IRepository<OnAim.Admin.Domain.Entities.User>> _mockUserRepository;
    private readonly Mock<IValidator<ChangePasswordCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;
    private readonly Mock<IPasswordService> _mockPasswordService;
    private readonly CommandContext<ChangePasswordCommand> _commandContext;

    public ChangePasswordCommandHandlerTest()
    {
        _mockUserRepository = new Mock<IRepository<Admin.Domain.Entities.User>>();
        _mockValidator = new Mock<IValidator<ChangePasswordCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();
        _mockPasswordService = new Mock<IPasswordService>();

        _commandContext = new CommandContext<ChangePasswordCommand>(
            _mockValidator.Object,
            _mockSecurityContextAccessor.Object
            );

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ChangePasswordCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldChangePassword_WhenUserExists()
    {
        var command = new ChangePasswordCommand("test@example.com", "OldPassword123", "NewPassword123");

        var user = new OnAim.Admin.Domain.Entities.User(
            "John", "Doe", "johndoe", "test@example.com",
            "hashedOldPassword", "salt", "1234567890", 1,
            isVerified: true, isActive: true,
            verificationCode: null, verificationPurpose: null,
            verificationCodeExpiration: null
        );

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new List<OnAim.Admin.Domain.Entities.User> { user }.AsQueryable().BuildMock());

        _mockPasswordService
            .Setup(e => e.EncryptPassword(command.OldPassword, user.Salt))
            .Returns(user.Password);

        _mockPasswordService
            .Setup(e => e.Salt())
            .Returns("newSalt");

        _mockPasswordService
            .Setup(e => e.EncryptPassword(command.NewPassword, "newSalt"))
            .Returns("hashedNewPassword");

        _mockUserRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new ChangePasswordCommandHandler(_commandContext, _mockUserRepository.Object, _mockPasswordService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("hashedNewPassword", user.Password);
        _mockUserRepository.Verify(repo => repo.CommitChanges(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoUsersFound()
    {
        var command = new ChangePasswordCommand("nonexistent@example.com", "OldPassword123", "NewPassword123");

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new List<OnAim.Admin.Domain.Entities.User>().AsQueryable().BuildMock());

        var handler = new ChangePasswordCommandHandler(_commandContext, _mockUserRepository.Object, _mockPasswordService.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

}
