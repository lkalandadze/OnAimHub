using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PlayerBanRepository(HubDbContext context) : BaseRepository<HubDbContext, PlayerBan>(context), IPlayerBanRepository
{
}