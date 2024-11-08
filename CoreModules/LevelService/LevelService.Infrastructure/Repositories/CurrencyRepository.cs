using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities.DbEnums;
using LevelService.Infrastructure.DataAccess;

namespace LevelService.Infrastructure.Repositories;

public class CurrencyRepository(LevelDbContext context) : BaseRepository<LevelDbContext, Currency>(context), ICurrencyRepository
{
}