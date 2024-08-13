using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.User;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PaginatedResult<UsersResponseModel>> GetAllUser(UserFilter userFilter)
        {
            var users = await _userRepository.GetAllUser(userFilter);
            return users;
        }

        public Task<int> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
