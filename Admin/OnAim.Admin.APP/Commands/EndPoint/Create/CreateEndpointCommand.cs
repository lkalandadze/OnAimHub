using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommand : IRequest<ApplicationResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? UserId { get; set; }
        public string? Type { get; set; }
    }
}
