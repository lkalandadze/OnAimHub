﻿using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class ConfigurationRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Base.Configuration>(context), IConfigurationRepository
{
}