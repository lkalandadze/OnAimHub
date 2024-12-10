using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Create;

public record CreatePromotionViewTemplateCommand(CreatePromotionViewTemplateAsyncDto Create) : ICommand<ApplicationResult>;
