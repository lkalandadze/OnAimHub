using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.Test.Endpoint;

public class DeleteEndpointCommandHandlerTest
{
    protected readonly Mock<IEndpointService> MockEndpointService;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;
    protected readonly Mock<IValidator<DeleteEndpointCommand>> MockValidator;

    public DeleteEndpointCommandHandlerTest()
    {
        MockEndpointService = new Mock<IEndpointService>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();
        MockValidator = new Mock<IValidator<DeleteEndpointCommand>>();

        MockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<DeleteEndpointCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    //[Fact]
    //public async Task Handle_ShouldDeleteDomains_WhenEndpointExist()
    //{
    //    var domainIds = new List<int> { 1, 2 };
    //    var domains = new List<OnAim.Admin.Domain.Entities.Endpoint>
    //    {
            
    //    }.AsQueryable().BuildMock();
    //    var command = new DeleteEndpointCommand(domainIds);

    //    var filter = new EndpointFilter();

    //    MockEndpointService
    //        .Setup(service => service.GetAll(filter))
    //        .ReturnsAsync(new ApplicationResult { Data = domains.AsQueryable() });

    //    MockEndpointService
    //        .Setup(service => service.Delete(It.IsAny<List<int>>()))
    //        .ReturnsAsync(new ApplicationResult { Success = true })
    //        .Verifiable();

    //    var handler = new DeleteEndpointCommandHandler(MockEndpointService.Object, MockValidator.Object);

    //    var result = await handler.Handle(command, CancellationToken.None);

    //    Assert.True(result.Success);
    //    MockEndpointService.Verify(service => service.Delete(It.IsAny<List<int>>()), Times.Once);
    //}

    //[Fact]
    //public async Task Handle_ShouldThrowNotFoundException_WhenNoEndpointsFound()
    //{
    //    var endpointIds = new List<int> { 3, 4 };
    //    var emptyEndpoints = new List<OnAim.Admin.Domain.Entities.Endpoint>().AsQueryable().BuildMock();
    //    var command = new DeleteEndpointCommand(endpointIds);
    //    var filter = new EndpointFilter();

    //    MockEndpointService
    //        .Setup(service => service.GetAll(filter))
    //        .ReturnsAsync(new ApplicationResult { Data = emptyEndpoints.AsQueryable() });

    //    MockEndpointService
    //         .Setup(service => service.Delete(It.IsAny<List<int>>()))
    //         .ReturnsAsync(new ApplicationResult { Success = true })
    //         .Verifiable();

    //    var handler = new DeleteEndpointCommandHandler(MockEndpointService.Object, MockValidator.Object);

    //    var result = await handler.Handle(command, CancellationToken.None);

    //    Assert.True(result.Success);
    //    MockEndpointService.Verify(service => service.Delete(It.IsAny<List<int>>()), Times.Once);
    //}
}
