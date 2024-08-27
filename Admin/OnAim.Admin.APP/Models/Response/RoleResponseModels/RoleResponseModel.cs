namespace OnAim.Admin.APP.Models.Response.Role
{
    public class RoleResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<EndpointGroupModeldTO> EndpointGroupModels { get; set; }
    }

    public class EndpointGroupModeldTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
