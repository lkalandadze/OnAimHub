using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public record CreatePromotionCommand(CreatePromotionDto Create) : ICommand<ApplicationResult<Guid>>;
