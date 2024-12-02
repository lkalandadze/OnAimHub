using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain;
using OnAim.Admin.Infrasturcture;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public record CreatePromotionCommand(CreatePromotionDto Command) : ICommand<ApplicationResult>;
