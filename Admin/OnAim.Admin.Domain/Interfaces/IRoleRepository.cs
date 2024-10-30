using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.Domain.Interfaces;

public interface IRoleRepository
{
    Task<PaginatedResult<RoleResponseModel>> GetAllRoles();
}
