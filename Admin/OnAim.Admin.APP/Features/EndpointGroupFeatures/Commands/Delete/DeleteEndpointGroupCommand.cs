using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public record DeleteEndpointGroupCommand(List<int> GroupIds) : ICommand<ApplicationResult>;
