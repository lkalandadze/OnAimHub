using OnAim.Admin.APP.Models.Response.EndpointModels;

namespace OnAim.Admin.APP.Models.Response.EndpointGroupResponseModels
{
    public class EndpointGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        public DateTimeOffset DateUpdated
        {
            get; set;
        }
    }
}
