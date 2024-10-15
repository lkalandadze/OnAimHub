using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IRoleService
{
    Task<ApplicationResult> Create(CreateRoleRequest request);
    Task<ApplicationResult> Delete(List<int> ids);
    Task<ApplicationResult> Update(int id, UpdateRoleRequest request);
    Task<ApplicationResult> GetAll(RoleFilter filter);
    Task<ApplicationResult> GetById(int id);
}
