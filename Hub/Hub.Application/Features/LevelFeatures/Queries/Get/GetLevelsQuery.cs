using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.LevelFeatures.Queries.Get;

public class GetLevelsQuery : PagedRequest, IRequest<GetLevelsQueryResponse>;