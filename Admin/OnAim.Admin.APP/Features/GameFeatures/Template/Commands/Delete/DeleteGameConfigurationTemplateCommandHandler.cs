using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;

public sealed class DeleteGameConfigurationTemplateCommandHandler : ICommandHandler<DeleteGameConfigurationTemplateCommand, ApplicationResult>
{
    private readonly IGameTemplateService _gameTemplateService;

    public DeleteGameConfigurationTemplateCommandHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult> Handle(DeleteGameConfigurationTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameTemplateService.DeleteGameConfigurationTemplate(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
