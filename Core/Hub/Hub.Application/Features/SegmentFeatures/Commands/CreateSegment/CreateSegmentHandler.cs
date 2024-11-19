using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;

public class CreateSegmentHandler : IRequestHandler<CreateSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;

    public CreateSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork, IMessageBus messageBus)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<Unit> Handle(CreateSegmentCommand request, CancellationToken cancellationToken)
    {
        var segments = _segmentRepository.Query(x => x.PriorityLevel == request.PriorityLevel);

        if (segments != null && segments.Any())
        {
            throw new ApiException(ApiExceptionCodeTypes.DuplicateEntry, "A segment with the same priority level already exists.");
        }

        var segment = new Segment(request.Id, request.Description, request.PriorityLevel, request.CreatedByUserId);

        await _segmentRepository.InsertAsync(segment);
        await _unitOfWork.SaveAsync();

        var @event = new CreateSegmentEvent(request.Id, Guid.NewGuid(), request.Description, request.PriorityLevel, false);

        await _messageBus.Publish(@event);

        return Unit.Value;
    }
}