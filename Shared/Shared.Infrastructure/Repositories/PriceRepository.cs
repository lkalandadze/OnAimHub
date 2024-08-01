using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PriceRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Price>(context), IPriceRepository
{
}