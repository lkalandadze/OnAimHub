using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities.DbEnums;
using LevelService.Infrastructure.DataAccess;

namespace LevelService.Infrastructure.Repositories;

public class PrizeTypeRepository(LevelDbContext context) : BaseRepository<LevelDbContext, PrizeType>(context), IPrizeTypeRepository
{
}