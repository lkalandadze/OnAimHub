using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PrizeClaimRepository(HubDbContext context) : BaseRepository<HubDbContext, Reward>(context), IRewardRepository
{
}