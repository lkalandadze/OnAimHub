using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Segment;

namespace Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;

public class DeleteSegmentHandler : IRequestHandler<DeleteSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;

    public DeleteSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork, IMessageBus messageBus)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<Unit> Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = _segmentRepository.Query(x => x.Id == request.Id).FirstOrDefault();

        if (segment == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Segment with the specified ID: [{request.Id}] was not found.");
        }

        segment.Delete();
        await _unitOfWork.SaveAsync();

        var @event = new DeleteSegmentEvent(Guid.NewGuid(), request.Id);
        await _messageBus.Publish(@event);

        return Unit.Value;
    }
}