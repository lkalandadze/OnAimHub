using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public record DeleteEndpointCommand(List<int> Ids) : ICommand<ApplicationResult>;
