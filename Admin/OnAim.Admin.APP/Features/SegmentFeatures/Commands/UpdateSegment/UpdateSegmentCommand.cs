using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;

public record UpdateSegmentCommand(string Id, string Description, int PriorityLevel) : ICommand<ApplicationResult>;
