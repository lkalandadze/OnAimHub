using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class CurrencyRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Currency>(context), ICurrencyRepository
{
}