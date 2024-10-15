using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentsForPlayers;

public record UnblockSegmentsForPlayersCommand(IEnumerable<string> SegmentIds, IFormFile File, int? ByUserId) : IRequest;