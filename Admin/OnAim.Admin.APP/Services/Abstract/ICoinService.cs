using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ICoinService
{
    Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter);
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> UpdateCoinForPromotion(List<string> promotionIds, string coinId, CoinDto updatedCoin);
}
