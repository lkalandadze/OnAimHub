using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayers;

public record UnassignSegmentToPlayersCommand(string SegmentId, IFormFile File, int? ByUserId) : IRequest;