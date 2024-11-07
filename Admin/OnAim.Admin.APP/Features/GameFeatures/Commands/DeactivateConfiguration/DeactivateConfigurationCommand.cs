using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;

public record DeactivateConfigurationCommand(int Id) : ICommand<ApplicationResult>;
