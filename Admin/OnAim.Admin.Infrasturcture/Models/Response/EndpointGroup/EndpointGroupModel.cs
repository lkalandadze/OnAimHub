using OnAim.Admin.Infrasturcture.Entities;

namespace OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup
{
    public class EndpointGroupModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<OnAim.Admin.Infrasturcture.Entities.Endpoint>? Endpoints { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
