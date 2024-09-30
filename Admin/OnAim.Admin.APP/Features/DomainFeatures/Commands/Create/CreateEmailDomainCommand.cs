using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public record CreateEmailDomainCommand(int? Id, string Domain, bool? IsActive) : ICommand<ApplicationResult>;
