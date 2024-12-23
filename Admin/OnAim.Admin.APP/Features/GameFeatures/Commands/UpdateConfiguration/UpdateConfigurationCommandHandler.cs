using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;

public class UpdateConfigurationCommandHandler : ICommandHandler<UpdateConfigurationCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public UpdateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.UpdateConfiguration(request.gameName, request.ConfigurationJson);

        return new ApplicationResult { Data = result };
    }
}
