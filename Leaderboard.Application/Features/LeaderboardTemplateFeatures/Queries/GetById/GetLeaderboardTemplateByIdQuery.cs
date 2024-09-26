using MediatR;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.GetById;


public record GetLeaderboardTemplateByIdQuery(int Id) : IRequest<GetLeaderboardTemplateByIdQueryResponse>;