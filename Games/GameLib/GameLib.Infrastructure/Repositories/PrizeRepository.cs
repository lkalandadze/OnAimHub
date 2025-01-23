using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class PrizeRepository<TPrize>(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, TPrize>(context), IPrizeRepository<TPrize>
    where TPrize : BasePrize
{

}