using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;

public class GetLeaderboardProgressQuery : PagedRequest, IRequest<GetLeaderboardProgressQueryResponse>;