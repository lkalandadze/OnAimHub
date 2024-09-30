using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayers;

public record UnblockSegmentForPlayersCommand(string SegmentId, IFormFile File, int? ByUserId) : IRequest;