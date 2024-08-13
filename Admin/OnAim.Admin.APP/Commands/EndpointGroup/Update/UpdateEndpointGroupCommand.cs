using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public record UpdateEndpointGroupCommand(string Id, UpdateEndpointGroupModel model) : IRequest<ApplicationResult>;

    public class UpdateEndpointGroupModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? EndpointIds { get; set; }
        public string? UserId { get; set; }
    }
}
