using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.Repositories;

public class PlayerRepository(HubDbContext context) : BaseRepository<HubDbContext, Player>(context), IPlayerRepository
{
    public async Task<Player?> GetPlayerWithSegmentsAsync(int id)
    {
        return await Query(p => p.Id == id).Include(x => x.PlayerSegments).FirstOrDefaultAsync();
    }
}