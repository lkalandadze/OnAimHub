using OnAim.Admin.Infrasturcture.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class Endpoint : BaseEntity
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int? UserId { get; set; }
        public EndpointType? Type { get; set; }
        public string Description { get; set; }
        public List<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; }
    }
}
