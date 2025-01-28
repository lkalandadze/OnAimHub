using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;

public class ActivateConfigurationCommandHandler : ICommandHandler<ActivateConfigurationCommand, object>
{
    private readonly IGameService _gameService;

    public ActivateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(ActivateConfigurationCommand request, CancellationToken cancellationToken)
    {
        return await _gameService.ActivateConfiguration(request.Name, request.Id);
    }
}
