﻿using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionViewRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionView>(context), IPromotionViewRepository
{
}