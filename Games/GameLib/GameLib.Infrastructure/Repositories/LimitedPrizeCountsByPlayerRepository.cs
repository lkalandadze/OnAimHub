using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class LimitedPrizeCountsByPlayerRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, LimitedPrizeCountsByPlayer>(context), ILimitedPrizeCountsByPlayerRepository
{

}