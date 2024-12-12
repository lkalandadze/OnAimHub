using Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Test;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;

public sealed class FinishLeaderboardCommandHandler : IRequestHandler<FinishLeaderboardCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    private readonly IMessageBus _messageBus;

    public FinishLeaderboardCommandHandler(
        ILeaderboardRecordRepository leaderboardRecordRepository,
        ILeaderboardProgressRepository leaderboardProgressRepository,
        ILeaderboardResultRepository leaderboardResultRepository,
        IMessageBus messageBus)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _messageBus = messageBus;
    }

    public async Task Handle(FinishLeaderboardCommand request, CancellationToken cancellationToken)
    {
        // Fetch the leaderboard record
        var leaderboardRecord = await _leaderboardRecordRepository.Query()
            .Include(x => x.LeaderboardRecordPrizes)
            .FirstOrDefaultAsync(x => x.Id == request.LeaderboardRecordId, cancellationToken);

        if (leaderboardRecord == default || leaderboardRecord.Status != LeaderboardRecordStatus.InProgress)
            throw new Exception($"Leaderboard record with ID {request.LeaderboardRecordId} is not in progress.");

        // Fetch all leaderboard progress
        var leaderboardProgress = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        if (!leaderboardProgress.Any())
            throw new Exception("No progress found for this leaderboard.");

        // Sort progress by amount
        var sortedProgress = leaderboardProgress
            .OrderByDescending(x => x.Amount)
            .ToList();

        // Generate rewards and save leaderboard results
        var rewardDetails = new List<RewardDetail>();
        int placement = 1;

        foreach (var progress in sortedProgress)
        {
            var prize = leaderboardRecord.LeaderboardRecordPrizes
                .FirstOrDefault(p => placement >= p.StartRank && placement <= p.EndRank);

            if (prize != null)
            {
                rewardDetails.Add(new RewardDetail(
                    progress.PlayerId,
                    prize.CoinId,
                    prize.Amount,
                    leaderboardRecord.PromotionId
                ));
            }

            var leaderboardResult = new LeaderboardResult(
                request.LeaderboardRecordId,
                progress.PlayerId,
                progress.PlayerUsername,
                placement++,
                progress.Amount
            );

            await _leaderboardResultRepository.InsertAsync(leaderboardResult);
        }

        await _leaderboardResultRepository.SaveChangesAsync();

        // Clear leaderboard progress (optional if clearing is required)
        // await _leaderboardProgressRepository.ClearLeaderboardProgressAsync(request.LeaderboardRecordId, cancellationToken);

        // Set leaderboard status to finished
        leaderboardRecord.Status = LeaderboardRecordStatus.Finished;
        await _leaderboardRecordRepository.SaveChangesAsync();

        // Publish reward event
        var @events = new ReceiveLeaderboardRewardEvent(Guid.NewGuid(), rewardDetails, DateTime.UtcNow);
        await _messageBus.Publish(@events);
    }
}
