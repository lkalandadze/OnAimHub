using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;

public class GetLeaderboardProgressQuery : PagedRequest, IRequest<GetLeaderboardProgressQueryResponse>;