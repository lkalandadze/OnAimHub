using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public record CreateEndpointCommand(List<CreateEndpointDto> Endpoints) : ICommand<ApplicationResult<string>>;
