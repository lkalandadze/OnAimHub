using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;

public class DeactivateConfigurationCommandHandler : ICommandHandler<DeactivateConfigurationCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public DeactivateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(DeactivateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.DeactivateConfiguration(request.Id);

        return new ApplicationResult { Data = result };
    }
}
