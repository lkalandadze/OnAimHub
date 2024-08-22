using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;

namespace OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup
{
    public class EndpointGroupModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public List<EndpointRequestModel>? Endpoints { get; set; }
        public int EndpointsCount { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
