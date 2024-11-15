﻿using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionCoinWithdrawOptionRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionCoinWithdrawOption>(context), IPromotionCoinWithdrawOptionRepository
{
}