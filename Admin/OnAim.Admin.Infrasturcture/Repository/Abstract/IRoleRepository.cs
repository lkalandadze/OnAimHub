using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Role;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IRoleRepository
    {
        Task DeleteRole(int roleId);
        Task<RoleResponseModel> GetRoleById(int roleId);
        Task<Role> CreateRoleAsync(CreateRoleRequest request);
        Task<RoleResponseModel> GetRoleByName(string roleName);
        Task<Role> UpdateRoleAsync(int roleId, UpdateRoleRequest request);
        Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<int> roleIds);
        Task<PaginatedResult<RoleResponseModel>> GetAllRoles(RoleFilter filter);
        Task<PaginatedResult<RoleResponseModel>> GetAllRolesAsync(RoleFilter filter);
    }
}
