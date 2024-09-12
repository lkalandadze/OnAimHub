using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ConsulLogRepository(HubDbContext context) : BaseRepository<HubDbContext, ConsulLog>(context), IConsulLogRepository
{
}