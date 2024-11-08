using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class LevelPrizeRepository(HubDbContext context) : BaseRepository<HubDbContext, LevelPrize>(context), ILevelPrizeRepository
{
}