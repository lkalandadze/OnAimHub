using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommand : ICommand<ApplicationResult>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Type { get; set; }
}
