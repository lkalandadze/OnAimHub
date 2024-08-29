using Hub.Domain.Wrappers;
using MediatR;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQuery : IRequest<List<Response<GetActiveGamesQueryResponse>>>
{
}