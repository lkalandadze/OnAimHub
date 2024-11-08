using MediatR;
using Shared.Lib.Wrappers;

namespace LevelService.Application.Features.StageFeatures.Queries.Get;

public class GetStageQuery : PagedRequest, IRequest<GetStageQueryResponse>;