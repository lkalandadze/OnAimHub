using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.Infrasturcture.Interfaces;

public interface IRoleRepository
{
    Task<PaginatedResult<RoleResponseModel>> GetAllRoles();
}
