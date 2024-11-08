using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Infrastructure.DataAccess;
using Shared.Lib.Helpers;

namespace GameLib.Infrastructure.Repositories;

public class PrizeGroupRepository<TPrizeGroup>(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, TPrizeGroup>(context), IPrizeGroupRepository<TPrizeGroup>
    where TPrizeGroup : BasePrizeGroup
{
    public List<TPrizeGroup> QueryWithTree()
    {
        var dbSet = RepositoryHelper.GetDbSet<TPrizeGroup>(context);
        var type = RepositoryHelper.GetDbSetEntityType<TPrizeGroup>(context);

        return dbSet.IncludeNotHiddenAll(type).ToList();
    }
}
