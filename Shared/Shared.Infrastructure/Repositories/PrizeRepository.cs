using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Base.Prize>(context), IPrizeRepository
{
}