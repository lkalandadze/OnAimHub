using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public class UpdateEndpointCommand : ICommand<ApplicationResult>
{
    public int Id { get; set; }
    public UpdateEndpointDto Endpoint { get; set; }
}
