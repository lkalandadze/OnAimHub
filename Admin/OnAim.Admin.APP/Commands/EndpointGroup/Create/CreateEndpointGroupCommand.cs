using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommand : ICommand<ApplicationResult>
    {
        public CreateEndpointGroupRequest Model { get; set; }
    }
}
