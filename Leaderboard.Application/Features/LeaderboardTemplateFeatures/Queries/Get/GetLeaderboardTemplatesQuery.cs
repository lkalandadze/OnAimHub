using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.Get;

public class GetLeaderboardTemplatesQuery : PagedRequest, IRequest<GetLeaderboardTemplatesQueryResponse>;