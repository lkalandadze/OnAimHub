using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetPlayerResults;

public class GetPlayerResultsQuery : PagedRequest, IRequest<GetPlayerResultsQueryResponse>
{
    public int PlayerId { get; }
}