using MissionService.Domain.Abstractions.Repository;
using MissionService.Domain.Entities;
using MissionService.Infrastructure.DataAccess;

namespace MissionService.Infrastructure.Repositories;

public class PlayerRepository(MissionDbContext context) : BaseRepository<MissionDbContext, Player>(context), IPlayerRepository
{
}