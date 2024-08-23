using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public class UpdateEndpointCommand : IRequest<ApplicationResult>
    {
        public int Id { get; set; }
        public UpdateEndpointDto Endpoint { get; set; }
    }
}
