using MediatR;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Commands.Create;

public sealed class CreateLeaderboardResultsCommand : IRequest
{
    public List<CreateLeaderboardResultsCommandItem> Results { get; set; }
}

public class CreateLeaderboardResultsCommandItem
{
    public int LeaderboardRecordId { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Placement { get; set; }
    public int Amount { get; set; }
}
