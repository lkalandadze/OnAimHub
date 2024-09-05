using Hub.Application.Features.PlayerFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersResponse : Response<PagedResponse<PlayerBaseDtoModel>>
{
}