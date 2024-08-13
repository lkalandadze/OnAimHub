namespace OnAim.Admin.API.Service
{
    public class EndpointInfo
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpMethod { get; set; }
        public string RouteTemplate { get; set; }
    }
}
