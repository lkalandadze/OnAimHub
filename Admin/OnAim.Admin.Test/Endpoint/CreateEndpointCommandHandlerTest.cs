using FluentValidation;
using Moq;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;

namespace OnAim.Admin.Test.Endpoint;

public class CreateEndpointCommandHandlerTest
{
    protected readonly Mock<IEndpointService> MockEndpointService;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;
    protected readonly Mock<IValidator<CreateEndpointCommand>> MockValidator;

    public CreateEndpointCommandHandlerTest()
    {
        MockEndpointService = new Mock<IEndpointService>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();
        MockValidator = new Mock<IValidator<CreateEndpointCommand>>();

        MockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateEndpointCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldCreateNewDomain_WhenEndpointDoesNotExist()
    {
        var command = new CreateEndpointCommand(new List<CreateEndpointDto>());

        var filter = new EndpointFilter("", true, HistoryStatus.All, null, null, null);

        MockEndpointService
            .Setup(service => service.GetAll(filter))
            .ReturnsAsync(new ApplicationResult { Data = new List<CreateEndpointDto>().AsQueryable() });

        MockEndpointService
            .Setup(service => service.Create(It.IsAny<List<CreateEndpointDto>>()))
            .ReturnsAsync(new ApplicationResult { Success = true })
            .Verifiable();

        var handler = new CreateEndpointCommandHandler(MockEndpointService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockEndpointService.Verify(service => service.Create(It.IsAny<List<CreateEndpointDto>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenEndpointExists()
    {
        var endpoints = new List<CreateEndpointDto> { };
        var command = new CreateEndpointCommand(endpoints);

        MockEndpointService.Setup(s => s.Create(command.Endpoints)).ThrowsAsync(new BadRequestException("endpoint already exists"));

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => MockEndpointService.Object.Create(command.Endpoints));
        Assert.Equal("endpoint already exists", exception.Message);
    }
}
