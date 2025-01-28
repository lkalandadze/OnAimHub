using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;

public class DeactivateConfigurationCommandHandler : ICommandHandler<DeactivateConfigurationCommand, object>
{
    private readonly IGameService _gameService;

    public DeactivateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(DeactivateConfigurationCommand request, CancellationToken cancellationToken)
    {
        return await _gameService.DeactivateConfiguration(request.Name, request.Id);
    }
}
