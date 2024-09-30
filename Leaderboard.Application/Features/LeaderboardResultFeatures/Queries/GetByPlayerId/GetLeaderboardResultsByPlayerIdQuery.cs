using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetByPlayerId;

public class GetLeaderboardResultsByPlayerIdQuery : PagedRequest, IRequest<GetLeaderboardResultsByPlayerIdQueryResponse>
{
    public int PlayerId { get; }
}