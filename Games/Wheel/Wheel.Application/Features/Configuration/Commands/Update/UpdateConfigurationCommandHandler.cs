using GameLib.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wheel.Application.Services.Abstract;

namespace Wheel.Application.Features.Configuration.Commands.Update;

public class UpdateConfigurationCommandHandler : IRequestHandler<UpdateConfigurationCommand, Unit>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IGameService _gameService;
    public UpdateConfigurationCommandHandler(IConfigurationRepository configurationRepository, IGameService gameService)
    {
        _configurationRepository = configurationRepository;
        _gameService = gameService;
    }

    public async Task<Unit> Handle(UpdateConfigurationCommand command, CancellationToken cancellationToken)
    {
        var configuration = await _configurationRepository.Query()
                        .Where(x => x.Id == command.Id)
                        .FirstOrDefaultAsync();

        if (configuration != default)
        {
            configuration.Update(command.Name, command.IsDefault, command.IsActive, command.GameVersionId);

            _configurationRepository.Update(configuration);
            await _configurationRepository.SaveAsync();
            await _gameService.UpdateMetadataAsync();
        }

        return Unit.Value;
    }
}
