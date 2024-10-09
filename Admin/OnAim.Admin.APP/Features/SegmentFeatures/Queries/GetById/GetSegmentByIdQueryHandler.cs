using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;

public sealed class GetSegmentByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<Segment> _segmentRepository;

    public GetSegmentByIdQueryHandler(IReadOnlyRepository<Segment> segmentRepository)
    {
        _segmentRepository = segmentRepository;
    }

    public async Task<ApplicationResult> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        var segment = await _segmentRepository
            .Query(x => x.Id == request.SegmentId)
            .Select(x => new
            {
                x.Id,
                x.Description,
                x.PriorityLevel
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (segment == null)
            throw new NotFoundException("Segment not found.");

        var res = new SegmentDto
        {
            SegmentId = segment.Id,
            SegmentName = segment.Id,
            PriorityLevel = segment.PriorityLevel,
        };

        return new ApplicationResult { Success = true, Data = res };
    }
}
