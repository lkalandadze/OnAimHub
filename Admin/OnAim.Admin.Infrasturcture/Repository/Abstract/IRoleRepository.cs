using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IRoleRepository
    {
        Task<PaginatedResult<RoleResponseModel>> GetAllRoles();
    }
}
