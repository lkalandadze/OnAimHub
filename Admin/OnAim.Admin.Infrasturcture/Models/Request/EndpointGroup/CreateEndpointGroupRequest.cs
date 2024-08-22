namespace OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup
{
    public class CreateEndpointGroupRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> EndpointIds { get; set; }

        public class CreateEndpointDto
        {
            public int Id { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
