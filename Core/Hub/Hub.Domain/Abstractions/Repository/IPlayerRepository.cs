using Hub.Domain.Entities;
using Shared.Domain.Entities;

namespace Hub.Domain.Abstractions.Repository;

public interface IPlayerRepository : IBaseRepository<Player>
{
    Task<Player?> GetPlayerWithSegmentsAsync(int id);

    Task<IEnumerable<int>> GetMissingPlayerIdsAsync(IEnumerable<int> playerIds);
}