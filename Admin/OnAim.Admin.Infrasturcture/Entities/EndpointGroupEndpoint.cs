namespace OnAim.Admin.Infrasturcture.Entities
{
    public class EndpointGroupEndpoint
    {
        public int EndpointGroupId { get; set; }
        public EndpointGroup EndpointGroup { get; set; }
        public int EndpointId { get; set; }
        public Endpoint Endpoint { get; set; }
        public bool IsActive { get; set; }
    }
}
