using MediatR;
using Shared.Lib.Wrappersl;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersQuery : PagedRequest, IRequest<GetPlayersResponse>
{
}