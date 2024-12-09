using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;
using Leaderboard.Application.Services.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Application.Services.Concrete;

public class LeaderboardService : ILeaderboardService
{
    public LeaderboardService()
    {
        
    }

    public void ValidateLeaderboardPrizes(List<CreateLeaderboardRecordPrizeCommandItem> prizes)
    {
        // Check for duplicate or overlapping ranges
        var sortedPrizes = prizes.OrderBy(p => p.StartRank).ToList();

        for (int i = 0; i < sortedPrizes.Count - 1; i++)
        {
            var current = sortedPrizes[i];
            var next = sortedPrizes[i + 1];

            if (current.EndRank >= next.StartRank)
                throw new ValidationException($"Prize ranking overlap detected between ranks {current.StartRank}-{current.EndRank} and {next.StartRank}-{next.EndRank}.");

            if (current.StartRank > current.EndRank)
                throw new ValidationException($"Invalid rank range: StartRank {current.StartRank} cannot be greater than EndRank {current.EndRank}.");
        }

        var last = sortedPrizes.Last();

        if (last.StartRank > last.EndRank)
            throw new ValidationException($"Invalid rank range: StartRank {last.StartRank} cannot be greater than EndRank {last.EndRank}.");
    }
}
