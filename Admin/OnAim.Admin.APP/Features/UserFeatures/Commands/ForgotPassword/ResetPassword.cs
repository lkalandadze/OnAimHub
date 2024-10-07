using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ResetPassword : ICommand<ApplicationResult>
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string Password { get; set; }
}
