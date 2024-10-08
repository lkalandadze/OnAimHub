using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;

public record AssignPlayerCommand(string SegmentId, int PlayerId) : ICommand<ApplicationResult>;
    
