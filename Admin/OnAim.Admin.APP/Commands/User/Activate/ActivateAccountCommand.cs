using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Activate
{
    public record ActivateAccountCommand(string Email, int Code) : ICommand<ApplicationResult>;
}