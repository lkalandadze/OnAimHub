using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents.IntegrationEvents.Leaderboard;
using System.Security.Claims;

namespace Leaderboard.Application.Consumers.Play;

public sealed class PlayLeaderboardAggregationConsumer : IConsumer<PlayLeaderboardEvent>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IPlayerRepository _playerRepository;

    public PlayLeaderboardAggregationConsumer(ILeaderboardRecordRepository leaderboardRecordRepository,
                                              ILeaderboardProgressRepository leaderboardProgressRepository,
                                              IPlayerRepository playerRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _playerRepository = playerRepository;
    }
    public async Task Consume(ConsumeContext<PlayLeaderboardEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var request = context.Message;

        var player = await _playerRepository.Query().FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

        if (player == default)
        {
            throw new InvalidOperationException("player not found");
        }

        var leaderboard = await _leaderboardRecordRepository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == request.LeaderboardRecordId, cancellationToken);

        if (leaderboard == null || leaderboard.Status != LeaderboardRecordStatus.InProgress)
        {
            throw new InvalidOperationException("Leaderboard not found or is not in progress.");
        }

        // Check and update leaderboard progress
        var existingProgress = await _leaderboardProgressRepository
            .GetProgressAsync(player.Id, request.LeaderboardRecordId, cancellationToken);

        if (existingProgress != null)
        {
            existingProgress.Amount += request.GeneratedAmount;

            await _leaderboardProgressRepository
                .SaveProgressAsync(existingProgress, TimeSpan.FromDays(7), cancellationToken);
        }
        else
        {
            var newProgress = new LeaderboardProgress(player.Id, player.UserName, request.GeneratedAmount)
            {
                LeaderboardRecordId = request.LeaderboardRecordId
            };

            await _leaderboardProgressRepository
                .SaveProgressAsync(newProgress, TimeSpan.FromDays(7), cancellationToken);
        }
    }
}