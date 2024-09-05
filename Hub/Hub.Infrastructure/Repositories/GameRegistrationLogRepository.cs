﻿using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class GameRegistrationLogRepository(HubDbContext context) : BaseRepository<HubDbContext, GameRegistrationLog>(context), IGameRegistrationLogRepository
{
}