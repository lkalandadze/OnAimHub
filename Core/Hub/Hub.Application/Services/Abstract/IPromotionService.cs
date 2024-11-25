using Hub.Domain.Enum;

namespace Hub.Application.Services.Abstract;

public interface IPromotionService
{
    Task UpdatePromotionStatus(int promotionId, PromotionStatus newStatus);
}