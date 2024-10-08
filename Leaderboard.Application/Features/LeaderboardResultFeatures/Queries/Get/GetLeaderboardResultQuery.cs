using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.Get;

public class GetLeaderboardResultQuery : PagedRequest, IRequest<GetLeaderboardResultQueryResponse>
{
    public int LeaderboardRecordId { get; }
}