using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs
{
    public class GetSegmentActsQueryHandler : IQueryHandler<GetSegmentActsQuery, ApplicationResult>
    {
        private readonly ISegmentService _segmentService;

        public GetSegmentActsQueryHandler(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        public async Task<ApplicationResult> Handle(GetSegmentActsQuery request, CancellationToken cancellationToken)
        {
            var result = await _segmentService.GetGeneralSegmentActs(request.Filter);

            return new ApplicationResult{ Success = result.Success, Data = result.Data };
        }
    }
}
