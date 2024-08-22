using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Role;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IRoleRepository
    {
        Task<PaginatedResult<RoleResponseModel>> GetAllRolesAsync(RoleFilter filter);
        Task<RoleResponseModel> GetRoleById(int roleId);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task RemoveRoleFromUserAsync(int userId, int roleId);
        Task DeleteRole(int roleId);
        Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<int> roleIds);
        Task<RoleResponseModel> GetRoleByName(string roleName);
        Task<Role> CreateRoleAsync(CreateRoleRequest request);
        Task<Role> UpdateRoleAsync(int roleId, UpdateRoleRequest request);
    }
}
