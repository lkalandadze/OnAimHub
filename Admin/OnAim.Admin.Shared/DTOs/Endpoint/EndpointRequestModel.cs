namespace OnAim.Admin.Shared.DTOs.Endpoint;

public class EndpointRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsEnabled { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
