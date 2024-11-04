using MassTransit;
using MissionService.Domain.Abstractions.Repository;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace MissionService.Application.Consumers.Segment;

public class CreateSegmentAggregationConsumer : IConsumer<CreateSegmentEvent>
{
    private readonly ISegmentRepository _segmentRepository;

    public CreateSegmentAggregationConsumer(ISegmentRepository segmentRepository)
    {
        _segmentRepository = segmentRepository;
    }
    public async Task Consume(ConsumeContext<CreateSegmentEvent> context)
    {
        var data = context.Message;

        var segment = new Domain.Entities.Segment(data.Id, data.Description, data.PriorityLevel, data.IsDeleted);

        await _segmentRepository.InsertAsync(segment);
        await _segmentRepository.SaveChangesAsync();
    }
}
