using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response.Role;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IRoleRepository
    {
        Task<List<RoleResponseModel>> GetAllRolesAsync();
        Task<RoleResponseModel> GetRoleById(string roleId);
        Task AssignRoleToUserAsync(string userId, string roleId);
        Task RemoveRoleFromUserAsync(string userId, string roleId);
        Task DeactivateRoleForUserAsync(string userId, string roleId);
        Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<string> roleIds);
        Task<Role> CreateRoleAsync(CreateRoleRequest request);
        Task<Role> UpdateRoleAsync(string roleId, UpdateRoleRequest request);
    }
}
