using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Active
{
    public record ActiveCommand(string Token) : ICommand<ApplicationResult>;
    public record SendEmailVerificationCode(string Email) : ICommand<ApplicationResult>
    {
        public static SendEmailVerificationCode Of(string? email) => new(email.NotBeEmptyOrNull());
    }
}
