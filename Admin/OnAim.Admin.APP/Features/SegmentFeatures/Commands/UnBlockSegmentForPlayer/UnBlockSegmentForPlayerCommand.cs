using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockPlayer;

public record UnBlockSegmentForPlayerCommand(string SegmentId, int PlayerId) : ICommand<ApplicationResult<bool>>;
