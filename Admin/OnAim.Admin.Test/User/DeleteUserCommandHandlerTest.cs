using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Test.User;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IRepository<OnAim.Admin.Domain.Entities.User>> _mockUserRepository;
    private readonly Mock<IValidator<DeleteUserCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;
    private readonly CommandContext<DeleteUserCommand> _commandContext;


    public DeleteUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<OnAim.Admin.Domain.Entities.User>>();
        _mockValidator = new Mock<IValidator<DeleteUserCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _commandContext = new CommandContext<DeleteUserCommand>(
             _mockValidator.Object,
             _mockSecurityContextAccessor.Object
         );

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<DeleteUserCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldDeleteUsers_WhenUsersExist()
    {
        var userIds = new List<int> { 1, 2 };
        var users = new List<OnAim.Admin.Domain.Entities.User>
        {
             new OnAim.Admin.Domain.Entities.User(
            "John",
            "Doe",
            "johndoe",
            "john.doe@example.com",
            "hashedpassword",
            "salt",
            "1234567890",
            createdBy: null,
            isVerified: true,
            isActive: true,
            verificationCode: null,
            verificationPurpose: null,
            verificationCodeExpiration: null,
            isSuperAdmin: false
        ),
        new OnAim.Admin.Domain.Entities.User(
            "Jane",
            "Doe",
            "janedoe",
            "jane.doe@example.com",
            "hashedpassword",
            "salt",
            "0987654321",
            createdBy: null,
            isVerified: true,
            isActive: true,
            verificationCode: null,
            verificationPurpose: null,
            verificationCodeExpiration: null,
            isSuperAdmin: false
            )
        }.AsQueryable().BuildMock();

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(users);

        _mockUserRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new DeleteUserCommandHandler(_commandContext, _mockUserRepository.Object);
        var command = new DeleteUserCommand(userIds);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockUserRepository.Verify(repo => repo.CommitChanges(), Times.Once);
        foreach (var user in users)
        {
            Assert.True(user.IsDeleted);
            Assert.False(user.IsActive);
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoUsersFound()
    {
        var userIds = new List<int> { 3, 4 };
        var emptyUsers = new List<OnAim.Admin.Domain.Entities.User>().AsQueryable().BuildMock();

        _mockUserRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(emptyUsers);

        var handler = new DeleteUserCommandHandler(_commandContext, _mockUserRepository.Object);
        var command = new DeleteUserCommand(userIds);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        _mockUserRepository.Verify(repo => repo.CommitChanges(), Times.Never);
    }
}
