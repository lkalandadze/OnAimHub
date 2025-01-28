using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Services.AdminServices.Role;

public interface IRoleService
{
    Task<ApplicationResult<string>> Create(CreateRoleRequest request);
    Task<ApplicationResult<bool>> Delete(List<int> ids);
    Task<ApplicationResult<string>> Update(int id, UpdateRoleRequest request);
    Task<ApplicationResult<PaginatedResult<RoleShortResponseModel>>> GetAll(RoleFilter filter);
    Task<ApplicationResult<RoleResponseModel>> GetById(int id);
}
