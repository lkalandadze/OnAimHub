using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Commands.User.Login
{
    public class LoginUserCommand : ICommand<AuthResultDto>
    {
        public LoginUserRequest Model { get; set; }

        public LoginUserCommand(LoginUserRequest model)
        {
            Model = model;
        }
    }
}
