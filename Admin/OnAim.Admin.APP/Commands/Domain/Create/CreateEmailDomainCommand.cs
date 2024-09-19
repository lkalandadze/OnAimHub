using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Domain.Create
{
    public record CreateEmailDomainCommand(int? Id, string Domain) : ICommand<ApplicationResult>;
}
