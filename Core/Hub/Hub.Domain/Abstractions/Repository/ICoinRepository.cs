using Hub.Domain.Entities.Coins;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface ICoinRepository : IBaseEntityRepository<Coin>
{
}