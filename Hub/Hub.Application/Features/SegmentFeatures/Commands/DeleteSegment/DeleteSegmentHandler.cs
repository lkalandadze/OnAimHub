using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;

public class DeleteSegmentHandler : IRequestHandler<DeleteSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = _segmentRepository.Query(x => x.Id == request.Id).FirstOrDefault();

        if (segment == null)
        {
            throw new KeyNotFoundException($"Segment not fount for Id: {request.Id}");
        }

        _segmentRepository.Delete(segment);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}