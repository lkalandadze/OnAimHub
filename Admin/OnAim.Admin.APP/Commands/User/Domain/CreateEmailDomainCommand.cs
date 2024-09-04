using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Domain
{
    public record CreateEmailDomainCommand(string Domain) : ICommand<ApplicationResult>;
}
