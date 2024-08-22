namespace OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup
{
    public class UpdateEndpointGroupRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public List<int>? EndpointIds { get; set; }
    }
}
