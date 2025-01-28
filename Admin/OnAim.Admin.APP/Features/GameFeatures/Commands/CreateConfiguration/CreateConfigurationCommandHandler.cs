using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;

public class CreateConfigurationCommandHandler : ICommandHandler<CreateConfigurationCommand, object>
{
    private readonly IGameService _gameService;

    public CreateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        return await _gameService.CreateConfiguration(request.gameName, request.ConfigurationJson);
    }
}
