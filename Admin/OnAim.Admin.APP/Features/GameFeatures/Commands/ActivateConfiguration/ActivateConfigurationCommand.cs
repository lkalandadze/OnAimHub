using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;

public record ActivateConfigurationCommand(string Name, int Id) : ICommand<object>;
