namespace OnAim.Admin.Contracts.Dtos.Role;

public class ExportRoleShortResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string EndpointGroups { get; set; }
}
