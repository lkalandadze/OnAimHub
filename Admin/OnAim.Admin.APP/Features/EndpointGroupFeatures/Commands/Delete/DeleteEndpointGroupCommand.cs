using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public record DeleteEndpointGroupCommand(int GroupId) : ICommand<ApplicationResult>;
