namespace OnAim.Admin.Shared.DTOs.Endpoint
{
    public class EndpointModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
    }
}
