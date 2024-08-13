namespace OnAim.Admin.Infrasturcture.Entities
{
    public class EndpointGroupEndpoint
    {
        public string EndpointGroupId { get; set; }
        public EndpointGroup EndpointGroup { get; set; }
        public string EndpointId { get; set; }
        public Endpoint Endpoint { get; set; }
    }
}
