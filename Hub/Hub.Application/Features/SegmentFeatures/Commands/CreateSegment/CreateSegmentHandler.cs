﻿using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;

public class CreateSegmentHandler : IRequestHandler<CreateSegmentCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSegmentHandler(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
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

        return Unit.Value;
    }
}