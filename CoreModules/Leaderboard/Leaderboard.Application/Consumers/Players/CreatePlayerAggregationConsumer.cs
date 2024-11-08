using Hub.Domain.Events;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MassTransit;

namespace Leaderboard.Application.Consumers.Players;

public sealed class CreatePlayerAggregationConsumer : IConsumer<CreatePlayerEvent>
{
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerAggregationConsumer(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    public async Task Consume(ConsumeContext<CreatePlayerEvent> context)
    {
        var data = context.Message;

        // Check for Duplicate Requests
        //var requestProcessed = _playerRepository.Query().FirstOrDefault(x => x.Id == data.PlayerId);

        //if (requestProcessed != null)
        //    throw new Exception("User already exists");

        var player = new Player(data.PlayerId, data.UserName);

        await _playerRepository.InsertAsync(player);
        await _playerRepository.SaveChangesAsync(context.CancellationToken);
    }
}
