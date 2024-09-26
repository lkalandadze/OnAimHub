using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public record CreateEmailDomainCommand(int? Id, string Domain) : ICommand<ApplicationResult>;
