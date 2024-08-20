using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommand : IRequest<ApplicationResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> EndpointIds { get; set; }
        public int? UserId { get; set; }

        public class CreateEndpointDto
        {
            public int Id { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
