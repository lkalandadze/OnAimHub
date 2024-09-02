using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class GameVersionRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, GameVersion>(context), IGameVersionRepository
{
}