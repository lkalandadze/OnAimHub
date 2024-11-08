using MediatR;
using Shared.Lib.Wrappers;

namespace LevelService.Application.Features.LevelFeatures.Queries.Get;

public sealed class GetLevelsQuery : PagedRequest, IRequest<GetLevelsQueryResponse>
{
    public int StageId { get; set; }
}