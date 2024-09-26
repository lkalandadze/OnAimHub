using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.Get;

public class GetLeaderboardTemplatesQuery : PagedRequest, IRequest<GetLeaderboardTemplatesQueryResponse>;