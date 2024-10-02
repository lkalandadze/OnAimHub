using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public record DeleteEmailDomainCommand(List<int> Ids) : ICommand<ApplicationResult>;
