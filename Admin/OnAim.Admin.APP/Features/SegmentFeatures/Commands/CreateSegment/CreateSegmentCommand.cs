using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;

public sealed record CreateSegmentCommand(string Id, string Description, int PriorityLevel) : ICommand<ApplicationResult>;
