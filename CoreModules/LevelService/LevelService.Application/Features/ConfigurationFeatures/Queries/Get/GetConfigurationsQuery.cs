using MediatR;
using Shared.Lib.Wrappers;

namespace LevelService.Application.Features.ConfigurationFeatures.Queries.Get;


public class GetConfigurationsQuery : PagedRequest, IRequest<GetConfigurationsQueryResponse>
{
    public int StageId { get; set; }
}