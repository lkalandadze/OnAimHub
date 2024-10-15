using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentsToPlayers;

public record UnassignSegmentsToPlayersCommand(IEnumerable<string> SegmentIds, IFormFile File, int? ByUserId) : IRequest;