namespace OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup
{
    public class EndpointGroupRequestModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> EndpointIds { get; set; }
    }
}
