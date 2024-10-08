using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using FluentValidation;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using MockQueryable;
using System.Linq.Expressions;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Test.User;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IRepository<OnAim.Admin.Domain.Entities.User>> _mockUserRepository;
    private readonly Mock<IRepository<Role>> _mockRoleRepository;
    private readonly Mock<IConfigurationRepository<UserRole>> _mockConfigurationRepository;
    private readonly CommandContext<CreateUserCommand> _commandContext;
    private readonly Mock<IValidator<CreateUserCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<OnAim.Admin.Domain.Entities.User>>();
        _mockRoleRepository = new Mock<IRepository<Role>>();
        _mockEmailService = new Mock<IEmailService>();
        _mockConfigurationRepository = new Mock<IConfigurationRepository<UserRole>>();
        _mockValidator = new Mock<IValidator<CreateUserCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();


        _commandContext = new CommandContext<CreateUserCommand>(
        _mockValidator.Object,
        _mockSecurityContextAccessor.Object);

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

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(emptyUserList);

        _mockUserRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new CreateUserCommandHandler(
            _commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockUserRepository.Verify(repo => repo.Store(It.IsAny<OnAim.Admin.Domain.Entities.User>()), Times.Once);
        _mockUserRepository.Verify(repo => repo.CommitChanges(), Times.Once);
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

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(existingList.BuildMock());

        var handler = new CreateUserCommandHandler(_commandContext,
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockConfigurationRepository.Object,
            _mockEmailService.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
    }
}
