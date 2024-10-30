using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;

public record BlockSegmentForPlayerCommand(string SegmentId, int PlayerId) : ICommand<ApplicationResult>;
