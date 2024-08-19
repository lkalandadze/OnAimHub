namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionForRoleAsync(string role, string permission);
    }
}
