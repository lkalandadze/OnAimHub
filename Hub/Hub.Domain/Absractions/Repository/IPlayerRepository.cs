﻿using Hub.Domain.Entities;

namespace Hub.Domain.Absractions.Repository;

public interface IPlayerRepository : IBaseRepository<Player>
{
    Task<Player?> GetPlayerWithSegmentsAsync(int id);

    Task<IEnumerable<int>> GetMissingPlayerIdsAsync(IEnumerable<int> playerIds);
}