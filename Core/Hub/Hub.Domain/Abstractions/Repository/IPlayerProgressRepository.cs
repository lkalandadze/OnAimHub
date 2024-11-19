using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface IPlayerProgressRepository : IBaseEntityRepository<PlayerProgress>
{
    void DeleteAll();
}