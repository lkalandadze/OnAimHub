using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;

public record DeleteSegmentCommand(string Id) : ICommand<ApplicationResult<bool>>;
