using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerBalanceRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerBalance>(context), IPlayerBalanceRepository
{

}