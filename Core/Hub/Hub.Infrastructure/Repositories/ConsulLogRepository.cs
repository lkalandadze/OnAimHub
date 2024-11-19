using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ConsulLogRepository(HubDbContext context) : BaseRepository<HubDbContext, ConsulLog>(context), IConsulLogRepository
{
}