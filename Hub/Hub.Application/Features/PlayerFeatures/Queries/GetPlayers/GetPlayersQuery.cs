using MediatR;
using Shared.Domain.Wrappers;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersQuery : PagedRequest, IRequest<GetPlayersResponse>
{
}