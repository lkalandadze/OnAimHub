using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;

public class CreateEndpointGroupCommand : ICommand<ApplicationResult<string>>
{
    public CreateEndpointGroupRequest Model { get; set; }
}
