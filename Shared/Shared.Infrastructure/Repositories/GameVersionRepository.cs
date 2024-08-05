using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class GameVersionRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, GameVersion>(context), IGameVersionRepository
{
}