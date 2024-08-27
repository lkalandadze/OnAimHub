﻿using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeGroupRepository<TPrizeGroup>(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, TPrizeGroup>(context), IPrizeGroupRepository<TPrizeGroup>
    where TPrizeGroup : BasePrizeGroup
{
    public List<TPrizeGroup> QueryWithPrizes()
    {
        return _context.Set<TPrizeGroup>().Where(x => x.Configuration.IsActive)
                                          .Include(x => x.Prizes)
                                          .Include(x => x.Configuration)
                                          .ToList();
    }
}
