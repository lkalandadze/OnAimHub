using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Services.Concrete;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PromotionService(IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task UpdatePromotionStatus(int promotionId, PromotionStatus newStatus)
    {
        var promotion = await _promotionRepository.Query()
            .FirstOrDefaultAsync(p => p.Id == promotionId);

        if (promotion == default)
            throw new Exception($"Promotion with ID {promotionId} not found.");

        if (promotion.Status == PromotionStatus.Paused || promotion.Status == PromotionStatus.Cancelled || promotion.Status == PromotionStatus.ToLaunch)
            throw new Exception($"Promotion with ID {promotionId} is {promotion.Status}, skipping status update to {newStatus}.");

        promotion.UpdateStatus(newStatus);

        _promotionRepository.Update(promotion);
        await _unitOfWork.SaveAsync();
    }
}
