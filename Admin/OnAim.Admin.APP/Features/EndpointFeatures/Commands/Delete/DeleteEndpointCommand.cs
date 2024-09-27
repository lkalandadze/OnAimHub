using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public record DeleteEndpointCommand(int Id) : ICommand<ApplicationResult>;
