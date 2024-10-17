using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ActRepository(HubDbContext context) : BaseRepository<HubDbContext, Act>(context), IActRepository
{
}