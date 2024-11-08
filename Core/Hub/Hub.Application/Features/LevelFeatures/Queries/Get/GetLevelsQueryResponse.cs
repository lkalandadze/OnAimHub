using Hub.Application.Features.LevelFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.LevelFeatures.Queries.Get;

public class GetLevelsQueryResponse : Response<PagedResponse<GetLevelsModel>>;