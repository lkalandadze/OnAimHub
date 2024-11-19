using Hub.Domain.Entities;
using Shared.Domain.Entities;

namespace Hub.Domain.Abstractions.Repository;

public interface IPlayerProgressRepository : IBaseRepository<PlayerProgress>
{
    void DeleteAll();
}