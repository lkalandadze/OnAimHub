using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Domain.Delete
{
    public record DeleteEmailDomainCommand(int Id) : ICommand<ApplicationResult>;
}
