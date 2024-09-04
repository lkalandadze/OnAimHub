using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public record DeleteEndpointCommand(int Id) : ICommand<ApplicationResult>;
}
