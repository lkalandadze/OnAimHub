using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Create;

public record CreatePromotionTemplateCommand(CreatePromotionTemplate Create) : ICommand<ApplicationResult>;
