using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentsToPlayers;

public record AssignSegmentsToPlayersCommand(IEnumerable<string> SegmentIds, IFormFile File, int? ByUserId) : IRequest;