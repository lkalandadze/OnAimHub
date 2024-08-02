using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeGroupRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Base.PrizeGroup>(context), IPrizeGroupRepository
{
}