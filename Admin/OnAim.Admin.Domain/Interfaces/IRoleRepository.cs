using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.Domain.Interfaces;

public interface IRoleRepository
{
    Task<PaginatedResult<RoleResponseModel>> GetAllRoles();
}
