using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs
{
    public class GetSegmentActsQueryHandler : IQueryHandler<GetSegmentActsQuery, ApplicationResult<PaginatedResult<ActsDto>>>
    {
        private readonly ISegmentService _segmentService;

        public GetSegmentActsQueryHandler(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        public async Task<ApplicationResult<PaginatedResult<ActsDto>>> Handle(GetSegmentActsQuery request, CancellationToken cancellationToken)
        {
            return await _segmentService.GetGeneralSegmentActs(request.Filter);
        }
    }
}
