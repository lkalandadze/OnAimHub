using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;

public record AssignSegmentToPlayerCommand(string SegmentId, int PlayerId, int? ByUserId) : IRequest;