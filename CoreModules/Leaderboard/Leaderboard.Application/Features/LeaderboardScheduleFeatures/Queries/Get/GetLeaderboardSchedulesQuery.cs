using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Queries.Get;

public class GetLeaderboardSchedulesQuery : PagedRequest, IRequest<GetLeaderboardSchedulesQueryResponse>;