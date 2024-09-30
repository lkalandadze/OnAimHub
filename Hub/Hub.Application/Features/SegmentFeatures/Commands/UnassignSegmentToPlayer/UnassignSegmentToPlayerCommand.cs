using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;

public record UnassignSegmentToPlayerCommand(string SegmentId, int PlayerId, int? ByUserId) : IRequest;