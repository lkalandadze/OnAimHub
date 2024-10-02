using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersQuery : PagedRequest, IRequest<GetPlayersResponse>
{
}