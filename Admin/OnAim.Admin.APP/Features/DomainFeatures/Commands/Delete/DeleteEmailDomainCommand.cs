using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public record DeleteEmailDomainCommand(int Id) : ICommand<ApplicationResult>;
