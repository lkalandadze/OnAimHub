using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.PromotionCoins;
using Hub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.Repositories;

public class PromotionCoinRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionCoin>(context), IPromotionCoinRepository
{
    public async Task<List<TCoin>> GetCoinsByTypeAsync<TCoin>(bool includeRelations = false) where TCoin : PromotionCoin
    {
        var query = _context.PromotionCoins.OfType<TCoin>();

        if (includeRelations)
        {
            query = query.Include(c => c.Promotion);

            switch (typeof(TCoin))
            {
                case Type t when t == typeof(PromotionOutgoingCoin):
                    query = query.Include(c => ((PromotionOutgoingCoin)(object)c).WithdrawOptions);
                    break;

                default: break;
            }
        }

        return await query.ToListAsync();
    }
}