using Hub.Application.Features.PlayerBanFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.GetById;

public class GetBannedPlayerByIdQueryResponse : Response<BannedPlayersDto>;