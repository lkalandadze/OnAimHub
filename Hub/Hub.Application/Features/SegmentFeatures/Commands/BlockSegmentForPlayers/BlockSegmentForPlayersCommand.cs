using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public record BlockSegmentForPlayersCommand(string SegmentId, IFormFile File, int? ByUserId) : IRequest;