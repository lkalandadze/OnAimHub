using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;

public record CreateConfigurationCommand(string gameName, GameConfigurationDto ConfigurationJson) : ICommand<ApplicationResult>;
