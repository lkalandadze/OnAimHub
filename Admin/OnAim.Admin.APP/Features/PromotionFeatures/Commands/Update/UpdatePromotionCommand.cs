using MongoDB.Bson;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Update;

public record UpdatePromotionCommand(UpdatePromotionDto Command) : ICommand<ApplicationResult>;
public record UpdatePromotionDto(ObjectId promotionId, OnAim.Admin.Domain.HubEntities.Promotion updatedPromotion);