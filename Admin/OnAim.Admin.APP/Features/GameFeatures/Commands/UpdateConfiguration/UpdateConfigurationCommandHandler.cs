using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;

public class UpdateConfigurationCommandHandler : ICommandHandler<UpdateConfigurationCommand, object>
{
    private readonly IGameService _gameService;

    public UpdateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        return await _gameService.UpdateConfiguration(request.gameName, request.ConfigurationJson);
    }
}
