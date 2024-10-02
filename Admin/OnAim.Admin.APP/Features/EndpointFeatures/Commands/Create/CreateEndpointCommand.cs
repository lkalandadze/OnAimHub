using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public record CreateEndpointCommand(List<CreateEndpointDto> Endpoints) : ICommand<ApplicationResult>;
