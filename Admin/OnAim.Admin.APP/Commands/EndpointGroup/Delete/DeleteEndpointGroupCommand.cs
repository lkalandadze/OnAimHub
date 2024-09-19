using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Delete
{
    public record DeleteEndpointGroupCommand(int GroupId) : ICommand<ApplicationResult>;
}
