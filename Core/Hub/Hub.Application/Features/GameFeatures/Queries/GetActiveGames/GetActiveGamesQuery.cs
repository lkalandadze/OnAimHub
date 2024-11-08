using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQuery : IRequest<List<Response<GetActiveGamesQueryResponse>>>
{
}