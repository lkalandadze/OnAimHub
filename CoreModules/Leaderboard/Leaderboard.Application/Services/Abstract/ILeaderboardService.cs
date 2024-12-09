using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

namespace Leaderboard.Application.Services.Abstract;

public interface ILeaderboardService
{
    void ValidateLeaderboardPrizes(List<CreateLeaderboardRecordPrizeCommandItem> prizes);
}
