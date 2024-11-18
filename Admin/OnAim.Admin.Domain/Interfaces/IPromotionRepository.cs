using MongoDB.Bson;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.Interfaces;

public interface IPromotionRepository
{
    Task<List<Promotion>> GetAllPromotionsAsync(PromotionFilter filter);
    Task<Promotion> GetPromotionByIdAsync(ObjectId promotionId);
    Task<Promotion> AddAsync(CreatePromotionDto promotion);
    Task<Promotion> UpdatePromotionAsync(ObjectId promotionId, Promotion updatedPromotion);
    Task<List<Promotion>> UpdateCoinForPromotionsAsync(List<string> promotionIds, string coinId, CoinDto updatedCoin);
}