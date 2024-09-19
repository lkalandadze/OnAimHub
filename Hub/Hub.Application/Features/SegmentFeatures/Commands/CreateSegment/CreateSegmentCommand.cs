using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;

public record CreateSegmentCommand(string Id, string Description, int PriorityLevel, int? CreatedByUserId) : IRequest;