using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Prize>(context), IPrizeRepository
{
}