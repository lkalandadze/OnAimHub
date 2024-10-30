using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public class UpdateEndpointCommand : ICommand<ApplicationResult>
{
    public int Id { get; set; }
    public UpdateEndpointDto Endpoint { get; set; }
}
