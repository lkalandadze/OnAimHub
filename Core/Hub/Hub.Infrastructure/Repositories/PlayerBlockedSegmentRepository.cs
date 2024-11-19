using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerBlockedSegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerBlockedSegment>(context), IPlayerBlockedSegmentRepository
{
}