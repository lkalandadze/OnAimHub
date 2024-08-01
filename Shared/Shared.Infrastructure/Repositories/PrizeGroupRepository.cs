using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeGroupRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Base.PrizeGroup>(context), IPrizeGroupRepository
{
}