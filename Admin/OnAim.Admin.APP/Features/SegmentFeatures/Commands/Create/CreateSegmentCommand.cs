using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create
{
    public sealed record CreateSegmentCommand(string Id, string Description, int PriorityLevel, int? CreatedByUserId) : ICommand<ApplicationResult>;
}
