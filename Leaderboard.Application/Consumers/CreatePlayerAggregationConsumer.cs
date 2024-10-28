using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Events;
using Shared.Infrastructure.Bus;

namespace Leaderboard.Application.Consumers;

public sealed class CreatePlayerAggregationConsumer : IMessageBusConsumer<CreatePlayerAggregationConsumer, CreatePlayerEvent>
{
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerAggregationConsumer(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    public async Task HandleAsync(CreatePlayerEvent data, MetaData metaData, CancellationToken cancellationToken = default)
    {
        //Check for Duplicate Requests
        var requestProcessed = _playerRepository.Query().FirstOrDefault(x => x.Id == data.PlayerId);

        if (requestProcessed != default)
            throw new Exception("User already exists");

        var player = new Player(data.PlayerId, data.UserName);

        await _playerRepository.InsertAsync(player);
        await _playerRepository.SaveChangesAsync(cancellationToken);
    }
}
