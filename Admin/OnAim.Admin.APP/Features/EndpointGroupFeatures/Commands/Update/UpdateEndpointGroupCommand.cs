using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public record UpdateEndpointGroupCommand(int Id, UpdateEndpointGroupRequest Model) : ICommand<ApplicationResult>;
