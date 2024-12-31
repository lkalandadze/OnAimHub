using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;

public class ActivateConfigurationCommandHandler : ICommandHandler<ActivateConfigurationCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public ActivateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(ActivateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.ActivateConfiguration(request.Name, request.Id);

        return new ApplicationResult { Data = result };
    }
}
