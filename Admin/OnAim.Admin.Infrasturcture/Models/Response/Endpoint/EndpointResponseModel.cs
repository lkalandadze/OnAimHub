using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Models.Response.Endpoint
{
    public class EndpointResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public EndpointType? Type { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
    }
}
