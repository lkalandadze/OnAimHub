using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;

public record CreateGameConfigurationTemplateCommand(CreateGameConfigurationTemplateDto Create) : ICommand<GameConfigurationTemplate>;
