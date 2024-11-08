using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerProgressRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerProgress>(context), IPlayerProgressRepository
{
    public void DeleteAll()
    {
        _context.PlayerProgresses.RemoveRange(_context.PlayerProgresses);
    }
}