using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;

public record ActivateConfigurationCommand(int Id) : ICommand<ApplicationResult>;
