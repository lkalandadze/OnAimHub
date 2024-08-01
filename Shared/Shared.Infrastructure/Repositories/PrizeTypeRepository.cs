using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeTypeRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Base.PrizeType>(context), IPrizeTypeRepository
{
}