using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using LevelService.Infrastructure.DataAccess;

namespace LevelService.Infrastructure.Repositories;

public class StageRepository(LevelDbContext context) : BaseRepository<LevelDbContext, Stage>(context), IStageRepository
{
}