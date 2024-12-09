using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Delete;

public record DeletePromotionViewTemplateCommand(string Id) : ICommand<ApplicationResult>;
