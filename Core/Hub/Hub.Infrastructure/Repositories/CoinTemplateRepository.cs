﻿using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Templates;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class CoinTemplateRepository(HubDbContext context) : BaseRepository<HubDbContext, CoinTemplate>(context), ICoinTemplateRepository
{
}