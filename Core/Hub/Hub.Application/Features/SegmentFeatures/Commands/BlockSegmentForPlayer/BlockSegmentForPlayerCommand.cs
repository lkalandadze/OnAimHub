using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;

public record BlockSegmentForPlayerCommand(string SegmentId, int PlayerId, int? ByUserId) : IRequest;