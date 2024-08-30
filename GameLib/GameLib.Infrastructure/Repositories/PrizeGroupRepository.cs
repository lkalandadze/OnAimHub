﻿using Microsoft.EntityFrameworkCore;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

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