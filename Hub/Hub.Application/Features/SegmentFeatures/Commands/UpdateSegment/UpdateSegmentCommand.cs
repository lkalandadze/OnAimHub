using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;

public record UpdateSegmentCommand(string Id, string Description, int PriorityLevel) : IRequest;