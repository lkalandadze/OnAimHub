using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace Leaderboard.Application.Consumers.Aggregation;

public sealed class AggregatedEventConsumer : IConsumer<AggregatedEvent>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IPlayerRepository _playerRepository;

    public AggregatedEventConsumer(ILeaderboardRecordRepository leaderboardRecordRepository,
                                              ILeaderboardProgressRepository leaderboardProgressRepository,
                                              IPlayerRepository playerRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _playerRepository = playerRepository;
    }

    public async Task Consume(ConsumeContext<AggregatedEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var request = context.Message;

        var playerId = int.Parse(request.PlayerId);
        var player = await _playerRepository.Query().FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player == default)
            throw new InvalidOperationException("player not found");

        var leaderboardExternalId = int.Parse(request.ConfigKey);
        var leaderboard = await _leaderboardRecordRepository
            .Query()
            .FirstOrDefaultAsync(x => x.ExternalId == leaderboardExternalId && x.Status == LeaderboardRecordStatus.InProgress , cancellationToken);

        //if (leaderboard == default)
        //    throw new Exception($"Leaderboard not found or is not in progress. leaderboardExternalId: {leaderboardExternalId} ---- LeaderboardId: {leaderboard.Id} ------ LeaderboardStatus: {leaderboard.Status}");

        var existingProgress = await _leaderboardProgressRepository
            .GetProgressAsync(player.Id, leaderboard.Id, cancellationToken);

        if (existingProgress != null)
        {
            existingProgress.Amount += (int)request.AddedPoints;

            await _leaderboardProgressRepository
                .SaveProgressAsync(existingProgress, TimeSpan.FromDays(7), cancellationToken);
        }
        else
        {
            var newProgress = new LeaderboardProgress(player.Id, player.UserName, (int)request.AddedPoints)
            {
                LeaderboardRecordId = leaderboard.Id
            };

            await _leaderboardProgressRepository
                .SaveProgressAsync(newProgress, TimeSpan.FromDays(7), cancellationToken);
        }
    }
}