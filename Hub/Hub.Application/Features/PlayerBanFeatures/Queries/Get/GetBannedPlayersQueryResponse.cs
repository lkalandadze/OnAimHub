using Hub.Application.Features.PlayerBanFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.Get;

public class GetBannedPlayersQueryResponse : Response<PagedResponse<BannedPlayersDto>>;