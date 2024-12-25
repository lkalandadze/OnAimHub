using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using FluentValidation;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.AdminServices.Domain;
namespace OnAim.Admin.Test.Domain;

public class CreateEmailDomainCommandHandlerTests
{
    private readonly Mock<IRepository<AllowedEmailDomain>> _mockRepository;
    protected readonly Mock<IDomainService> MockService;
    protected readonly Mock<IValidator<CreateEmailDomainCommand>> MockValidator;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;

    public CreateEmailDomainCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<AllowedEmailDomain>>();
        MockService = new Mock<IDomainService>();
        MockValidator = new Mock<IValidator<CreateEmailDomainCommand>>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        SetupDefaults();
    }

    private void SetupDefaults()
    {
        MockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateEmailDomainCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldCreateNewDomain_WhenDomainDoesNotExist()
    {
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);
        var filter = new DomainFilter();

        MockService
            .Setup(service => service.GetById(0))
            .ReturnsAsync(new Admin.Domain.Entities.AllowedEmailDomain("newdomain.com", 1));

        MockService
            .Setup(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()))
            .ReturnsAsync(new ApplicationResult { Success = true })
            .Verifiable();

        var handler = new CreateEmailDomainCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenDomainAlreadyExists()
    {
        var existingDomain = new DomainDto { Domain = "newdomain.com", Id = 1, IsActive = true, IsDeleted = false };
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "newdomain.com", true);
        var existingList = new List<DomainDto> { existingDomain }.AsQueryable();

        var filter = new DomainFilter();

        MockService
             .Setup(service => service.GetAllDomain(filter))
             .ReturnsAsync(new ApplicationResult { Data = existingList });

        MockService
               .Setup(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()))
               .ReturnsAsync(new ApplicationResult { Success = true })
               .Verifiable();

        var handler = new CreateEmailDomainCommandHandler(MockService.Object, MockValidator.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateDeletedDomain_WhenDomainExistsButDeleted()
    {
        var deletedDomain = new DomainDto { Domain = "newdomain.com", Id = 1, IsActive = true, IsDeleted = true };
        var command = new CreateEmailDomainCommand(new List<DomainDto>(), "deletedomain.com", true);
        var deletedList = new List<DomainDto> { deletedDomain }.AsQueryable();

        var filter = new DomainFilter();

        MockService
               .Setup(service => service.GetAllDomain(filter))
               .ReturnsAsync(new ApplicationResult { Data = deletedList });

        MockService
               .Setup(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()))
               .ReturnsAsync(new ApplicationResult { Success = true })
               .Verifiable();

        var handler = new CreateEmailDomainCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.CreateOrUpdateDomain(It.IsAny<List<DomainDto>>(), It.IsAny<string>(), It.IsAny<bool?>()), Times.Once);
    }
}
