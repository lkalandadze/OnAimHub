using Leaderboard.Domain.Abstractions.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace Leaderboard.Application.Consumers.Segment;

public class UpdateSegmentAggregationConsumer : IConsumer<UpdateSegmentEvent>
{
    private readonly ISegmentRepository _segmentRepository;

    public UpdateSegmentAggregationConsumer(ISegmentRepository segmentRepository)
    {
        _segmentRepository = segmentRepository;
    }
    public async Task Consume(ConsumeContext<UpdateSegmentEvent> context)
    {
        var data = context.Message;

        var existedSegment = await _segmentRepository.Query(x => x.Id == data.Id).FirstOrDefaultAsync();

        if (existedSegment != null)
        {
            existedSegment.Id = data.Id;
            existedSegment.Description = data.Description;
            existedSegment.PriorityLevel = data.PriorityLevel;

            await _segmentRepository.SaveChangesAsync();
        }
        else
        {
            var segment = new Domain.Entities.Segment(data.Id, data.Description, data.PriorityLevel, false);

            await _segmentRepository.InsertAsync(segment);
            await _segmentRepository.SaveChangesAsync();
        }
    }
}
