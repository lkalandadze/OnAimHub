using Hub.Application.Features.GameFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameResponse : Response<PagedResponse<GameBaseDtoModel>>
{
}