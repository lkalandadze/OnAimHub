using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;

public sealed class DeleteGameConfigurationTemplateCommandHandler : ICommandHandler<DeleteGameConfigurationTemplateCommand, ApplicationResult<bool>>
{
    private readonly IGameTemplateService _gameTemplateService;

    public DeleteGameConfigurationTemplateCommandHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteGameConfigurationTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _gameTemplateService.DeleteGameConfigurationTemplate(request.Id);
    }
}
