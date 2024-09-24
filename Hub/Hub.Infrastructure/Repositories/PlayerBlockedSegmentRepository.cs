using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerBlockedSegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerBlockedSegment>(context), IPlayerBlockedSegmentRepository
{
}