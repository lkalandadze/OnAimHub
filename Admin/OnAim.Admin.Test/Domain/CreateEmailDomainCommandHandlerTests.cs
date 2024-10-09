using System.Linq.Expressions;
using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.DTOs.EmailDomain;
using MockQueryable;
namespace OnAim.Admin.Test.Domain;

public class CreateEmailDomainCommandHandlerTests : TestsBase<CreateEmailDomainCommand, AllowedEmailDomain>
{
    [Fact]
    public async Task Handle_ShouldCreateNewDomain_WhenDomainDoesNotExist()
    {
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);

        MockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(new List<AllowedEmailDomain>().AsQueryable().BuildMock());

        MockRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new CreateEmailDomainCommandHandler(CommandContext, MockRepository.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockRepository.Verify(repo => repo.Store(It.IsAny<AllowedEmailDomain>()), Times.Once);
        MockRepository.Verify(repo => repo.CommitChanges(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenDomainAlreadyExists()
    {
        var existingDomain = new AllowedEmailDomain("newdomain.com", 1);
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);
        var existingList = new List<AllowedEmailDomain> { existingDomain }.AsQueryable();

        MockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(existingList.BuildMock());

        var handler = new CreateEmailDomainCommandHandler(CommandContext, MockRepository.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUpdateDeletedDomain_WhenDomainExistsButDeleted()
    {
        var deletedDomain = new AllowedEmailDomain("deletedomain.com", 1) { IsDeleted = true };
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "deletedomain.com", true);
        var deletedList = new List<AllowedEmailDomain> { deletedDomain }.AsQueryable();

        MockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
            .Returns(deletedList.BuildMock());

        MockRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new CreateEmailDomainCommandHandler(CommandContext, MockRepository.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockRepository.Verify(repo => repo.CommitChanges(), Times.Once);
        Assert.False(deletedDomain.IsDeleted);
    }
}
