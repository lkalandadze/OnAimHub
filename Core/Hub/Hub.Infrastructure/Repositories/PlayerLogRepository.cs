using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerLogRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerLog>(context), IPlayerLogRepository
{
}