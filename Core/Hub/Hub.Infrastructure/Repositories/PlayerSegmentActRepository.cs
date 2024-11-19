using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerSegmentActRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerSegmentAct>(context), IPlayerSegmentActRepository
{
}