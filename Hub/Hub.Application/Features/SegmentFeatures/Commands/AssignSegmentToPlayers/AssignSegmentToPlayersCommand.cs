using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayers;

public record AssignSegmentToPlayersCommand(string SegmentId, IFormFile File, int? ByUserId) : IRequest;