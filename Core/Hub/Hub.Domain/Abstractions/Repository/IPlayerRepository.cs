using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface IPlayerRepository : IBaseEntityRepository<Player>
{
    Task<Player?> GetPlayerWithSegmentsAsync(int id);

    Task<IEnumerable<int>> GetMissingPlayerIdsAsync(IEnumerable<int> playerIds);
}