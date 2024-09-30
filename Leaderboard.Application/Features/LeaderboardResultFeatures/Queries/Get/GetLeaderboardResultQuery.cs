using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.Get;

public class GetLeaderboardResultQuery : PagedRequest, IRequest<GetLeaderboardResultQueryResponse>
{
    public int LeaderboardRecordId { get; }
}