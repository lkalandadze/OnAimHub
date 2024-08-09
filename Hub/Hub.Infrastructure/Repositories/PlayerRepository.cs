using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerRepository(HubDbContext context) : BaseRepository<HubDbContext, Player>(context), IPlayerRepository
{
}