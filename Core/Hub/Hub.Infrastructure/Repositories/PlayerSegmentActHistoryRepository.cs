using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerSegmentActHistoryRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerSegmentActHistory>(context), IPlayerSegmentActHistoryRepository
{
}