using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;

namespace OnAim.Admin.Test.Domain;

public class DeleteEmailDomainCommandHandlerTest
{
    protected readonly Mock<IDomainService> MockService;
    protected readonly Mock<IValidator<DeleteEmailDomainCommand>> MockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;

    public DeleteEmailDomainCommandHandlerTest()
    {
         MockService = new Mock<IDomainService>();
        MockValidator = new Mock<IValidator<DeleteEmailDomainCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
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
        var command = new DeleteEmailDomainCommand(domainIds);

        var filter = new DomainFilter("");

        MockService
            .Setup(service => service.GetAllDomain(filter))
            .ReturnsAsync(new ApplicationResult { Data = new List<DomainDto>().AsQueryable() });

        MockService
            .Setup(service => service.DeleteEmailDomain(It.IsAny<List<int>>()))
            .ReturnsAsync(new ApplicationResult { Success = true })
            .Verifiable();

        var handler = new DeleteEmailDomainCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.DeleteEmailDomain(It.IsAny<List<int>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoDomainsFound()
    {
        var domainIds = new List<int> { 3, 4 };
        var emptyDomains = new List<OnAim.Admin.Domain.Entities.AllowedEmailDomain>().AsQueryable().BuildMock();
        var command = new DeleteEmailDomainCommand(domainIds);
        var filter = new DomainFilter("");

        MockService
            .Setup(service => service.GetAllDomain(filter))
            .ReturnsAsync(new ApplicationResult { Data = new List<DomainDto>().AsQueryable() });

        MockService
             .Setup(service => service.DeleteEmailDomain(It.IsAny<List<int>>()))
             .ReturnsAsync(new ApplicationResult { Success = true })
             .Verifiable();

        var handler = new DeleteEmailDomainCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.DeleteEmailDomain(It.IsAny<List<int>>()), Times.Once);
    }
}
