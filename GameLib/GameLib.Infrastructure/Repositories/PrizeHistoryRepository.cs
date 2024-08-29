using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class PrizeHistoryRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, PrizeHistory>(context), IPrizeHistoryRepository
{
}