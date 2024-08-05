using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class CurrencyRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Currency>(context), ICurrencyRepository
{
}