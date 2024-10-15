using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentsForPlayers;

public record BlockSegmentsForPlayersCommand(IEnumerable<string> SegmentIds, IFormFile File, int? ByUserId) : IRequest;