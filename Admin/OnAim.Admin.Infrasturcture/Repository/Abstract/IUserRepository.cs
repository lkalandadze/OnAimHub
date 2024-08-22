using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Models.Response.User;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IUserRepository
    {
        Task CommitChanges();
        Task DeleteUser(int userId);
        Task<User> Create(User user);
        Task<UsersResponseModel> GetById(int id);
        Task<User> FindByEmailAsync(string email);
        Task ResetPassword(int id, string password);
        Task<User> UpdateUser(int id, UpdateUserRequest user);
        Task<List<RoleResponseModel>> GetUserRolesAsync(int userId);
        Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
        Task<PaginatedResult<UsersResponseModel>> GetAllUser(UserFilter userFilter);
    }
}
