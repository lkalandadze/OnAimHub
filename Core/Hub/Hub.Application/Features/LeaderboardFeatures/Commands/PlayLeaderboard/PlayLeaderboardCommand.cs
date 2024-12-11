using MediatR;

namespace Hub.Application.Features.LeaderboardFeatures.Commands.PlayLeaderboard;

public sealed class PlayLeaderboardCommand : IRequest
{
    public int LeaderboardRecordId { get; set; }
    public int GeneratedAmount { get; set; }
    public int PromotionId { get; set; }
}