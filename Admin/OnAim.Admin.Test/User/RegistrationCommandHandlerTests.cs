using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Test.User;

public class RegistrationCommandHandlerTests
{
    private readonly Mock<IRepository<OnAim.Admin.Domain.Entities.User>> _mockUserRepository;
    private readonly Mock<IRepository<Role>> _mockRoleRepository;
    private readonly Mock<IConfigurationRepository<UserRole>> _mockConfigurationRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IDomainValidationService> _mockDomainValidationService;
    private readonly Mock<IValidator<RegistrationCommand>> _mockValidator;
    private readonly CommandContext<RegistrationCommand> _commandContext;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public RegistrationCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<OnAim.Admin.Domain.Entities.User>>();
        _mockRoleRepository = new Mock<IRepository<Role>>();
        _mockConfigurationRepository = new Mock<IConfigurationRepository<UserRole>>();
        _mockEmailService = new Mock<IEmailService>();
        _mockDomainValidationService = new Mock<IDomainValidationService>();
        _mockValidator = new Mock<IValidator<RegistrationCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _commandContext = new CommandContext<RegistrationCommand>(
        _mockValidator.Object,
         _mockSecurityContextAccessor.Object);

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

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new List<OnAim.Admin.Domain.Entities.User>().BuildMock());

        var handler = new RegistrationCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockUserRepository.Verify(repo => repo.Store(It.IsAny<OnAim.Admin.Domain.Entities.User>()), Times.Once);
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

        var handler = new RegistrationCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object
        );

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
            Shared.Enums.VerificationPurpose.AccountActivation,
            new DateTime(),
            false);
        var command = new RegistrationCommand
        {
            Email = "jane.doe@example.com"
        };

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(true);

        _mockUserRepository
             .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
             .Returns(new List<OnAim.Admin.Domain.Entities.User> { existingUser }.AsQueryable().BuildMock());

        var handler = new RegistrationCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object
        );

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
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
            Shared.Enums.VerificationPurpose.AccountActivation,
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

        _mockUserRepository
             .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
             .Returns(new List<OnAim.Admin.Domain.Entities.User> { existingUser }.AsQueryable().BuildMock());

        var handler = new RegistrationCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object
        );

        await handler.Handle(command, CancellationToken.None);

        _mockUserRepository.Verify(repo => repo.Store(It.IsAny<OnAim.Admin.Domain.Entities.User>()), Times.Once);
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
            Shared.Enums.VerificationPurpose.AccountActivation,
            DateTime.UtcNow,
            false)
        { IsDeleted = true };

        var command = new RegistrationCommand
        {
            Email = "deleted.user@example.com",
            Password = "12345",
        };

        var deletedList = new List<OnAim.Admin.Domain.Entities.User> { deletedUser };

        _mockUserRepository
            .Setup(repo => repo.QueryAsync(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .ReturnsAsync(deletedList.AsQueryable().BuildMock());

        _mockDomainValidationService
            .Setup(d => d.IsDomainAllowedAsync(command.Email))
            .ReturnsAsync(true);

        var handler = new RegistrationCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object
        );

        await handler.Handle(command, CancellationToken.None);

        _mockUserRepository.Verify(repo => repo.Store(It.IsAny<OnAim.Admin.Domain.Entities.User>()), Times.Once);
    }
}