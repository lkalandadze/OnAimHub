using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class PriceRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Price>(context), IPriceRepository
{
}