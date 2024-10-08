using System.Linq.Expressions;
using FluentValidation;
using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.DTOs.EmailDomain;
using MockQueryable;

namespace OnAim.Admin.Test.Domain;

public class CreateEmailDomainCommandHandlerTests
{
    private readonly Mock<IRepository<AllowedEmailDomain>> _mockRepository;
    private readonly Mock<IValidator<CreateEmailDomainCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;
    private readonly CommandContext<CreateEmailDomainCommand> _commandContext;

    public CreateEmailDomainCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<AllowedEmailDomain>>();
        _mockValidator = new Mock<IValidator<CreateEmailDomainCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _commandContext = new CommandContext<CreateEmailDomainCommand>(
            _mockValidator.Object,
            _mockSecurityContextAccessor.Object
        );

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateEmailDomainCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldCreateNewDomain_WhenDomainDoesNotExist()
    {
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);

        _mockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(new List<AllowedEmailDomain>().AsQueryable().BuildMock());

        _mockRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new CreateEmailDomainCommandHandler(_commandContext, _mockRepository.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockRepository.Verify(repo => repo.Store(It.IsAny<AllowedEmailDomain>()), Times.Once);
        _mockRepository.Verify(repo => repo.CommitChanges(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenDomainAlreadyExists()
    {
        var existingDomain = new AllowedEmailDomain("newdomain.com", 1);
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);
        var existingList = new List<AllowedEmailDomain> { existingDomain }.AsQueryable();

        _mockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(existingList.BuildMock());

        var handler = new CreateEmailDomainCommandHandler(_commandContext, _mockRepository.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUpdateDeletedDomain_WhenDomainExistsButDeleted()
    {
        var deletedDomain = new AllowedEmailDomain("deletedomain.com", 1) { IsDeleted = true };
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "deletedomain.com", true);
        var deletedList = new List<AllowedEmailDomain> { deletedDomain }.AsQueryable();

        _mockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(deletedList.BuildMock());

        _mockRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new CreateEmailDomainCommandHandler(_commandContext, _mockRepository.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockRepository.Verify(repo => repo.CommitChanges(), Times.Once);
        Assert.False(deletedDomain.IsDeleted);
    }


}
