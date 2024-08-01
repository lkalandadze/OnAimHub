using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class CurrencyRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Base.Currency>(context), ICurrencyRepository
{
}