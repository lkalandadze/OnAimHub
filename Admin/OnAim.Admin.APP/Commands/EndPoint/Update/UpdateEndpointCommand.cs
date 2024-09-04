using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public class UpdateEndpointCommand : ICommand<ApplicationResult>
    {
        public int Id { get; set; }
        public UpdateEndpointDto Endpoint { get; set; }
    }
}
