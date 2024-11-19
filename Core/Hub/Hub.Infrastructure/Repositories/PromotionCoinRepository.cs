using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionCoinRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionCoin>(context), IPromotionCoinRepository
{
}