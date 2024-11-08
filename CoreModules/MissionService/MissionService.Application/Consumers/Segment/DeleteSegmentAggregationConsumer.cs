using MassTransit;
using Microsoft.EntityFrameworkCore;
using MissionService.Domain.Abstractions.Repository;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace MissionService.Application.Consumers.Segment;

public class DeleteSegmentAggregationConsumer : IConsumer<DeleteSegmentEvent>
{
    private readonly ISegmentRepository _segmentRepository;

    public DeleteSegmentAggregationConsumer(ISegmentRepository segmentRepository)
    {
        _segmentRepository = segmentRepository;
    }
    public async Task Consume(ConsumeContext<DeleteSegmentEvent> context)
    {
        var data = context.Message;

        var segment = await _segmentRepository.Query(x => x.Id == data.SegmentId).FirstOrDefaultAsync();

        if (segment != null)
        {
            segment.IsDeleted = true;
            await _segmentRepository.SaveChangesAsync();
        }
    }
}
