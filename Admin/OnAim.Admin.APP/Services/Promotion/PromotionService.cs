using MongoDB.Bson;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Interfaces;

namespace OnAim.Admin.APP.Services.Promotion;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _repo;

    public PromotionService(IPromotionRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApplicationResult> GetAllPromotions(PromotionFilter filter)
    {
       var promotions = await _repo.GetAllPromotionsAsync(filter);
        return new ApplicationResult
        {
            Success = true,
            Data = promotions
        };
    }

    public async Task<ApplicationResult> GetPromotionById(ObjectId id)
    {
        var promotion = await _repo.GetPromotionByIdAsync(id);

        if (promotion == null) throw new NotFoundException("promotion not found");

        return new ApplicationResult { Success = true, Data = promotion };
    }

    public async Task<ApplicationResult> CreatePromotion(CreatePromotionDto create)
    {
        var createdPromotion = await _repo.AddAsync(create);

        return new ApplicationResult { Success = true, Data = createdPromotion };
    }

    public async Task<ApplicationResult> UpdatePromotion(ObjectId promotionId, OnAim.Admin.Domain.HubEntities.Promotion updatedPromotion)
    {
        await _repo.UpdatePromotionAsync(promotionId, updatedPromotion);

        return new ApplicationResult { Success = true, Data = "Promotion updated successfully" };
    }

}