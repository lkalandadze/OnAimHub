using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Login;

public class LoginUserCommand : ICommand<AuthResultDto>
{
    public LoginUserRequest Model { get; set; }

    public LoginUserCommand(LoginUserRequest model)
    {
        Model = model;
    }
}
