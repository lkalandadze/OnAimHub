using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;

public record AssignSegmentToPlayersCommand(string SegmentId, IFormFile File) : ICommand<ApplicationResult>;
