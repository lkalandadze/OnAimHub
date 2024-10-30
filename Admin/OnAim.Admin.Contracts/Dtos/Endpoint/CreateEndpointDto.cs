namespace OnAim.Admin.Contracts.Dtos.Endpoint;

public class CreateEndpointDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Type { get; set; }
}
