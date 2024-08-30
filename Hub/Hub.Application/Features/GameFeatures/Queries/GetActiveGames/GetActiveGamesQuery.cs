using MediatR;
using Shared.Domain.Wrappers;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQuery : IRequest<List<Response<GetActiveGamesQueryResponse>>>
{
}