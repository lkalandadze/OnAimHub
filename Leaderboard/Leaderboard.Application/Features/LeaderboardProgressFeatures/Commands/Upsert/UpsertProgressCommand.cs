using MediatR;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Commands.Upsert;

public sealed class UpsertProgressCommand : IRequest
{
    public int LeaderboardRecordId { get; set; }
    public int GeneratedAmount { get; set; }
}
