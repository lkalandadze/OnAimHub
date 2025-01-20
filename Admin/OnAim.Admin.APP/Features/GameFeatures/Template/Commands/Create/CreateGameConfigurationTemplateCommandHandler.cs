using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;

public sealed class CreateGameConfigurationTemplateCommandHandler : ICommandHandler<CreateGameConfigurationTemplateCommand, GameConfigurationTemplate>
{
    private readonly IGameTemplateService _gameTemplateService;

    public CreateGameConfigurationTemplateCommandHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<GameConfigurationTemplate> Handle(CreateGameConfigurationTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _gameTemplateService.CreateGameConfigurationTemplate(request.GameName, request.Create);
    }
}
