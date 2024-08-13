using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommand : IRequest<ApplicationResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> EndpointIds { get; set; }
        public string? UserId { get; set; }

        public class CreateEndpointDto
        {
            public string Id { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
