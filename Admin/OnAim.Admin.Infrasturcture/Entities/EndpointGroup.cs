using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class EndpointGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; }
        public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
        public bool IsEnabled { get; set; }
        public int? UserId { get; set; }
    }
}
