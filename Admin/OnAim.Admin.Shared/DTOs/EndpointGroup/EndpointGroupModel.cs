using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.Shared.DTOs.EndpointGroup;

public class EndpointGroupModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<EndpointRequestModel>? Endpoints { get; set; }
    public int EndpointsCount { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
