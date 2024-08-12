using Hub.Application.Features.PlayerFeatures.Dtos;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersResponse
{
    public IEnumerable<PlayerBaseDtoModel>? Players { get; set; }
}