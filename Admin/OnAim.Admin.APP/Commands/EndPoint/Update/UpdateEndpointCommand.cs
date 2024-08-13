using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Models.Response.EndpointModels;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public class UpdateEndpointCommand : IRequest<ApplicationResult>
    {
        public string Id { get; set; }
        public EndpointModel Endpoint { get; set; }
    }
}
