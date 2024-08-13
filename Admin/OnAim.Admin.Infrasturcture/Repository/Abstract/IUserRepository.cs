using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Models.Response.User;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<UsersResponseModel> GetById(string id);
        Task<User> FindByEmailAsync(string email);
        Task<PaginatedResult<UsersResponseModel>> GetAllUser(UserFilter userFilter);
        Task<List<RoleResponseModel>> GetUserRolesAsync(string userId);
        Task<IEnumerable<string>> GetUserPermissionsAsync(string userId);
    }
}
