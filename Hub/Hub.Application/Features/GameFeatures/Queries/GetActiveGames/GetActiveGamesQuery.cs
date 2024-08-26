using MediatR;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQuery : IRequest<List<GetActiveGamesQueryResponse>>
{
}
