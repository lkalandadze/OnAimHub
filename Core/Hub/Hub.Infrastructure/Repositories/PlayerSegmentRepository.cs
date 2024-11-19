using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerSegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerSegment>(context), IPlayerSegmentRepository
{
}