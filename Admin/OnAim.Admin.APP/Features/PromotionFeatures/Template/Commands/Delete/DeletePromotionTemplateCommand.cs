using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Delete;

public record DeletePromotionTemplateCommand(string Id) : ICommand<ApplicationResult>;
