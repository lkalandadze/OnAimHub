using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Test.Domain;

public class DeleteEmailDomainCommandHandlerTest
{
    private readonly Mock<IRepository<OnAim.Admin.Domain.Entities.AllowedEmailDomain>> _mockRepository;
    private readonly Mock<IValidator<DeleteEmailDomainCommand>> _mockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;
    private readonly CommandContext<DeleteEmailDomainCommand> _commandContext;

    public DeleteEmailDomainCommandHandlerTest()
    {
        _mockRepository = new Mock<IRepository<Admin.Domain.Entities.AllowedEmailDomain>>();
        _mockValidator = new Mock<IValidator<DeleteEmailDomainCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _commandContext = new CommandContext<DeleteEmailDomainCommand>(
             _mockValidator.Object,
             _mockSecurityContextAccessor.Object
         );

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<DeleteEmailDomainCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldDeleteDomains_WhenDomainExist()
    {
        var domainIds = new List<int> { 1, 2 };
        var domains = new List<OnAim.Admin.Domain.Entities.AllowedEmailDomain>
        {
             new OnAim.Admin.Domain.Entities.AllowedEmailDomain("test.gmail", 1),
             new OnAim.Admin.Domain.Entities.AllowedEmailDomain("testtwo.gmail", 1)
        }.AsQueryable().BuildMock();

        _mockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.AllowedEmailDomain, bool>>>()))
            .Returns(domains);

        _mockRepository
            .Setup(repo => repo.CommitChanges())
            .Returns(Task.CompletedTask);

        var handler = new DeleteEmailDomainCommandHandler(_commandContext, _mockRepository.Object);
        var command = new DeleteEmailDomainCommand(domainIds);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        _mockRepository.Verify(repo => repo.CommitChanges(), Times.Once);
        foreach (var user in domains)
        {
            Assert.True(user.IsDeleted);
            Assert.False(user.IsActive);
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoDomainsFound()
    {
        var domainIds = new List<int> { 3, 4 };
        var emptyDomains = new List<OnAim.Admin.Domain.Entities.AllowedEmailDomain>().AsQueryable().BuildMock();

        _mockRepository
            .Setup(repo => repo.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.AllowedEmailDomain, bool>>>()))
            .Returns(emptyDomains);

        var handler = new DeleteEmailDomainCommandHandler(_commandContext, _mockRepository.Object);
        var command = new DeleteEmailDomainCommand(domainIds);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        _mockRepository.Verify(repo => repo.CommitChanges(), Times.Never);
    }
}
