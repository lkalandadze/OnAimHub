using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using LevelService.Infrastructure.DataAccess;

namespace LevelService.Infrastructure.Repositories;

public class PlayerRepository(LevelDbContext context) : BaseRepository<LevelDbContext, Player>(context), IPlayerRepository
{
}