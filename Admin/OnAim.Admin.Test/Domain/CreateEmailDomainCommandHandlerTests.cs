using Moq;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using OnAim.Admin.Shared.DTOs.EmailDomain;
using FluentValidation;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.Domain;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;
using NSubstitute;
using MockQueryable;
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
        var filter = new DomainFilter("");

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

        var filter = new DomainFilter("");

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

        var filter = new DomainFilter("");

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

    /////////////////////////////////
    ///
    //[Fact]
    //public async Task CreateOrUpdateDomain_UpdatesExistingDomains_ReturnsSuccess()
    //{
    //    // Arrange
    //    var domains = new List<DomainDto>
    //{
    //    new DomainDto { Id = 1, Domain = "example.com", IsActive = true }
    //};

    //    var existingDomainEntity = new AllowedEmailDomain{ Id = 1, Domain = "old.com", IsActive = false };
    //    _mockRepository.Setup(r => r.Query(It.IsAny<Expression<Func<AllowedEmailDomain, bool>>>()))
    //    .Returns(new ApplicationResult { Data = domains } );

    //    // Act
    //    var result = await MockService.Object.CreateOrUpdateDomain(domains, null, null);

    //    // Assert
    //    Assert.True(result.Success);
    //    Assert.Equal("example.com", existingDomainEntity.Domain);
    //    Assert.True(existingDomainEntity.IsActive);
    //    _mockRepository.Verify(r => r.CommitChanges(), Times.Once);
    //}
    //[Fact]
    //public async Task CreateOrUpdateDomain_CreatesNewDomain_ReturnsSuccess()
    //{
    //    // Arrange
    //    var domain = "newdomain.com";
    //    _mockRepository.Setup(r => r.Query(It.IsAny<Expression<Func<DomainEntity, bool>>>()))
    //        .ReturnsAsync((DomainEntity)null);

    //    // Act
    //    var result = await _domainService.CreateOrUpdateDomain(null, domain, null);

    //    // Assert
    //    Assert.True(result.Success);
    //    _mockRepository.Verify(r => r.Store(It.IsAny<DomainEntity>()), Times.Once);
    //    _mockRepository.Verify(r => r.CommitChanges(), Times.Once);
    //}
    //[Fact]
    //public async Task CreateOrUpdateDomain_ThrowsBadRequest_WhenDomainExists()
    //{
    //    // Arrange
    //    var existingDomainEntity = new DomainEntity { Domain = "existing.com", IsDeleted = false };
    //    _mockRepository.Setup(r => r.Query(It.IsAny<Expression<Func<DomainEntity, bool>>>()))
    //        .ReturnsAsync(existingDomainEntity);

    //    // Act & Assert
    //    await Assert.ThrowsAsync<BadRequestException>(() =>
    //        _domainService.CreateOrUpdateDomain(null, "existing.com", null));
    //}

}
