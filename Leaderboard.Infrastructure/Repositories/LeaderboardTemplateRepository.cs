﻿using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardTemplateRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardTemplate>(context), ILeaderboardTemplateRepository
{
}