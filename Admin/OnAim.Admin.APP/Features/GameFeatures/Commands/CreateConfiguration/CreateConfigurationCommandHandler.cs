using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;

public class CreateConfigurationCommandHandler : ICommandHandler<CreateConfigurationCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public CreateConfigurationCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.CreateConfiguration(request.gameName, request.ConfigurationJson);

        return new ApplicationResult { Data = result, Success = true };
    }
}
