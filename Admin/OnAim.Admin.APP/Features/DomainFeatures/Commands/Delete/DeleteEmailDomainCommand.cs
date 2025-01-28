using OnAim.Admin.APP.CQRS.Command;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public record DeleteEmailDomainCommand(List<int> Ids) : ICommand<ApplicationResult<bool>>;
