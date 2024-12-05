using MediatR;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Test;

public sealed class FinishLeaderboardCommand : IRequest
{
    public int LeaderboardRecordId { get; set; }
}