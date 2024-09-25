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

    //TODO: should be fix
    public async Task<IEnumerable<int>> GetMissingPlayerIdsAsync(IEnumerable<int> playerIds)
    {
        string valuesClause = string.Join(", ", playerIds.Select(id => $"({id})"));

        string script = $@"SELECT value FROM (VALUES {valuesClause}) AS numbers(value)
                           WHERE value NOT IN (SELECT ""{nameof(Player.Id)}"" FROM ""{nameof(HubDbContext.Players)}"")";

        var missingPlayerIds = await _context.Database.ExecuteSqlRawAsync(script);

        return [];
    }
}