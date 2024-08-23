using OnAim.Admin.Infrasturcture.Attributes;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Models.Request.Endpoint
{
    public class EndpointFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEnable { get; set; }
        public EndpointType? Type { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
    }
    public class UpdateEndpointDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
