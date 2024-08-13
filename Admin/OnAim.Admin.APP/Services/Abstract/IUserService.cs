using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.User;

namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IUserService
    {
        Task<int> GetUserById(int id);
        Task<string> GetUserNameByEmail(string email);
        Task<PaginatedResult<UsersResponseModel>> GetAllUser(UserFilter userFilter);
    }
}
