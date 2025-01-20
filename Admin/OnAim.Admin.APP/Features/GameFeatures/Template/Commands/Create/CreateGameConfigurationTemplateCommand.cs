using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;

public record CreateGameConfigurationTemplateCommand(string GameName, object Create) : ICommand<GameConfigurationTemplate>;
