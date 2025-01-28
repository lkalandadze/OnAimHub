using MongoDB.Bson;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Update;

public record UpdatePromotionCommand(UpdatePromotionDto Command) : ICommand<ApplicationResult<object>>;
public record UpdatePromotionDto(ObjectId promotionId, Promotion updatedPromotion);