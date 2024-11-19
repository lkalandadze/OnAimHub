using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;

public class UpdateSegmentHandler : IRequestHandler<UpdateSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;

    public UpdateSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork, IMessageBus messageBus)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<Unit> Handle(UpdateSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = _segmentRepository.Query(x => x.Id == request.Id).FirstOrDefault();

        if (segment == null)
        {
            throw new KeyNotFoundException($"Segment not fount for Id: {request.Id}");
        }

        var segments = _segmentRepository.Query(x => x.PriorityLevel == request.PriorityLevel);

        if (segments != null && segments.Any())
        {
            throw new ApiException(ApiExceptionCodeTypes.DuplicateEntry, "A segment with the same priority level already exists.");
        }

        segment.ChangeDetails(request.Description, request.PriorityLevel);

        _segmentRepository.Update(segment);
        await _unitOfWork.SaveAsync();

        var @event = new UpdateSegmentEvent(request.Id, Guid.NewGuid(), request.Description, request.PriorityLevel);

        await _messageBus.Publish(@event);

        return Unit.Value;
    }
}