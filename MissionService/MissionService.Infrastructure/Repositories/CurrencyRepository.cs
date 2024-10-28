using MissionService.Domain.Abstractions.Repository;
using MissionService.Domain.Entities.DbEnums;
using MissionService.Infrastructure.DataAccess;

namespace MissionService.Infrastructure.Repositories;

public class CurrencyRepository(MissionDbContext context) : BaseRepository<MissionDbContext, Currency>(context), ICurrencyRepository
{
}