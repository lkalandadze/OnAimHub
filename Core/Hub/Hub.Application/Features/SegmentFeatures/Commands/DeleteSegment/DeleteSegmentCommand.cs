using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;

public record DeleteSegmentCommand(string Id) : IRequest;