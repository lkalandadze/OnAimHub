﻿using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;

public class UpdateSegmentHandler : IRequestHandler<UpdateSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
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

        return Unit.Value;
    }
}