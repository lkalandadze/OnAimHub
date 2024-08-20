using MediatR;
using OnAim.Admin.APP.Models.Response.EndpointModels;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public class UpdateEndpointCommand : IRequest<ApplicationResult>
    {
        public int Id { get; set; }
        public EndpointModel Endpoint { get; set; }
    }
}
