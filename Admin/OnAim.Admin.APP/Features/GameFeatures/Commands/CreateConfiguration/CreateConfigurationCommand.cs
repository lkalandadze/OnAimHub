using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;

public record CreateConfigurationCommand(string gameName, object ConfigurationJson) : ICommand<object>;
