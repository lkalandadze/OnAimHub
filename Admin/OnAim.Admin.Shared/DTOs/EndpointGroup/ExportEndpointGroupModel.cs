namespace OnAim.Admin.Shared.DTOs.EndpointGroup;

public class ExportEndpointGroupModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public string? Endpoints { get; set; }
    public int EndpointsCount { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
