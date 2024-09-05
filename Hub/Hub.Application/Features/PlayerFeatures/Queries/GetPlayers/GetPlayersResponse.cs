using Hub.Application.Features.PlayerFeatures.Dtos;
using Shared.Domain.Wrappers;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersResponse : PagedResponse<GetPlayersResponse>
{
    public IEnumerable<PlayerBaseDtoModel>? Players { get; set; }
}