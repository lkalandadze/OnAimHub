using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;

public record UpdateConfigurationCommand(string gameName, GameConfigurationDto ConfigurationJson) : ICommand<object>;
