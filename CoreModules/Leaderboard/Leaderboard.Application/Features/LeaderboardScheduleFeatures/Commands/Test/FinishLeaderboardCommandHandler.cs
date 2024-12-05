using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Test;

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
        var leaderboardRecord = await _leaderboardRecordRepository.Query()
            .Include(x => x.LeaderboardRecordPrizes)
            .FirstOrDefaultAsync(x => x.Id == request.LeaderboardRecordId, cancellationToken);

        if (leaderboardRecord == null || leaderboardRecord.Status != LeaderboardRecordStatus.Finished)
        {
            throw new Exception($"Leaderboard record with ID {request.LeaderboardRecordId} is not finished.");
        }

        var leaderboardProgress = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        if (!leaderboardProgress.Any())
        {
            throw new Exception("No progress found for this leaderboard.");
        }

        var sortedProgress = leaderboardProgress
            .OrderByDescending(x => x.Amount)
            .ToList();

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

        //await _leaderboardProgressRepository.ClearLeaderboardProgressAsync(request.LeaderboardRecordId, cancellationToken);

        var @events = new ReceiveLeaderboardRewardEvent(Guid.NewGuid(), rewardDetails, DateTime.UtcNow);

        await _messageBus.Publish(@events);
    }
}