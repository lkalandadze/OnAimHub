using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class PriceRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Base.Price>(context), IPriceRepository
{
}