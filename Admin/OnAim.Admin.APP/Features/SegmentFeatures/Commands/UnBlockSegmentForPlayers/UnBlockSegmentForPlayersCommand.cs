using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockSegmentForPlayers
{
    public record UnBlockSegmentForPlayersCommand(string SegmentId, IFormFile File) : ICommand<ApplicationResult>;
}
