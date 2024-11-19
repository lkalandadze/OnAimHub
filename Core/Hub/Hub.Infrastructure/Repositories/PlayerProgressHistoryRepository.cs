using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerProgressHistoryRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerProgressHistory>(context), IPlayerProgressHistoryRepository
{
}