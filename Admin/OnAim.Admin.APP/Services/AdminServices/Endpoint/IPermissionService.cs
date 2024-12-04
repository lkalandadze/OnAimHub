namespace OnAim.Admin.APP.Services.AdminServices.Endpoint;

public interface IPermissionService
{
    Task<bool> RolesContainPermission(List<string> roles, string permission);
}
