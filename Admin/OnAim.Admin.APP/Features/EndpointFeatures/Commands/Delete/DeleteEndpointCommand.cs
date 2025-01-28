using OnAim.Admin.APP.CQRS.Command;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public record DeleteEndpointCommand(List<int> Ids) : ICommand<ApplicationResult<bool>>;
