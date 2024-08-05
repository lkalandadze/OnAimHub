using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeHistoryRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, PrizeHistory>(context), IPrizeHistoryRepository
{
}