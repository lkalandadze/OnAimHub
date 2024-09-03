using GameLib.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wheel.Application.Services.Abstract;

namespace Wheel.Application.Features.GameVersion.Commands.Update;

public class UpdateGameVersionCommandHandler : IRequestHandler<UpdateGameVersionCommand, Unit>
{
    private readonly IGameVersionRepository _gameVersionRepository;
    private readonly IGameService _gameService;
    public UpdateGameVersionCommandHandler(IGameVersionRepository gameVersionRepository, IGameService gameService)
    {
        _gameVersionRepository = gameVersionRepository;
        _gameService = gameService;
    }

    public async Task<Unit> Handle(UpdateGameVersionCommand command, CancellationToken cancellationToken)
    {
        var game = await _gameVersionRepository.Query()
                        .Where(x => x.Id == command.Id)
                        .FirstOrDefaultAsync();

        if (game != default)
        {
            game.Update(command.Name, command.IsActive, command.SegmentIds);

            _gameVersionRepository.Update(game);
            await _gameVersionRepository.SaveAsync();

            await _gameService.UpdateMetadataAsync();
        }


        return Unit.Value;
    }
}
