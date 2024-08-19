using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Models.Response.Endpoint
{
    public class EndpointResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public string? Type { get; set; }
        public int? UserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
    }
}
