﻿using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PrizeTypeRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, PrizeType>(context), IPrizeTypeRepository
{
}