using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions.Repository;

namespace GameLib.Application.Services.Concrete;

public class SegmentService : ISegmentService
{
    private readonly ISegmentRepository _segmentRepository;

    public SegmentService(ISegmentRepository segmentRepository)
    {
        _segmentRepository = segmentRepository;
    }
}