using Hub.Domain.Entities;

namespace Hub.Domain.Absractions.Repository;

public interface IPlayerProgressRepository : IBaseRepository<PlayerProgress>
{
    void DeleteAll();
}