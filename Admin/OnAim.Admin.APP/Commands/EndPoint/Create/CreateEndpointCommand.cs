using MediatR;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommand : ICommand<ApplicationResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Type { get; set; }
    }
}
