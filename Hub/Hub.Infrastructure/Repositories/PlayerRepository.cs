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

    public async Task<IEnumerable<int>> GetMissingPlayerIdsAsync(IEnumerable<int> playerIds)
    {
        string valuesClause = string.Join(", ", playerIds.Select(id => $"({id})"));

        string script = $@"SELECT numbers.value AS ""Id""
                           FROM (VALUES {valuesClause}) AS numbers(value)
                           LEFT JOIN ""{nameof(HubDbContext.Players)}"" AS players
                           ON numbers.value = players.""{nameof(Player.Id)}""
                           WHERE players.""{nameof(Player.Id)}"" IS NULL";

        return await _context.Players
            .FromSqlRaw(script)
            .Select(p => p.Id)
            .ToListAsync();
    }
}