using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;

public record UnblockSegmentForPlayerCommand(string SegmentId, int PlayerId, int? ByUserId) : IRequest;