using Hub.Application.Features.PlayerFeatures.Dtos;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;

public class GetPlayerResponse
{
    public PlayerBaseDtoModel? Player { get; set; }
}