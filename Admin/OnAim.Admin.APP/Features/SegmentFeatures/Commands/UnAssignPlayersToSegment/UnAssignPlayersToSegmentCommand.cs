using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayersToSegment;

public record UnAssignPlayersToSegmentCommand(IEnumerable<string> SegmentId, IFormFile File) : ICommand<ApplicationResult>;
