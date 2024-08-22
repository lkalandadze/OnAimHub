namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IPermissionService
    {
        Task<bool> RolesContainPermission(List<string> roles, string permission);
    }
}
