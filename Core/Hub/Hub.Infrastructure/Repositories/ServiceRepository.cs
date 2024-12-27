using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ServiceRepository(HubDbContext context) : BaseRepository<HubDbContext, Service>(context), IServiceRepository
{
}