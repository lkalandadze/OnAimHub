using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public record CreatePromotionCommand(CreatePromotionDto Command) : ICommand<ApplicationResult>;
