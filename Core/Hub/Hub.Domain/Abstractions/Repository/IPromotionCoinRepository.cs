using Hub.Domain.Entities.PromotionCoins;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface IPromotionCoinRepository : IBaseEntityRepository<PromotionCoin>
{
}