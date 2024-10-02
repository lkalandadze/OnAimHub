using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer
{
    public record UnAssignPlayerCommand(string SegmentId, int PlayerId) : ICommand<ApplicationResult>;
}
