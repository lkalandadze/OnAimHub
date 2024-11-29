using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Coins;
using Hub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.Repositories;

public class CoinRepository(HubDbContext context) : BaseRepository<HubDbContext, Coin>(context), ICoinRepository
{
    public async Task<List<TCoin>> GetCoinsByTypeAsync<TCoin>(bool includeRelations = false) where TCoin : Coin
    {
        var query = _context.Coins.OfType<TCoin>();

        if (includeRelations)
        {
            query = query.Include(c => c.Promotion);

            switch (typeof(TCoin))
            {
                case Type t when t == typeof(OutCoin):
                    query = query.Include(c => ((OutCoin)(object)c).WithdrawOptions);
                    break;

                default: break;
            }
        }

        return await query.ToListAsync();
    }
}