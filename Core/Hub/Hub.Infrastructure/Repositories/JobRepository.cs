using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class JobRepository(HubDbContext context) : BaseRepository<HubDbContext, Job>(context), IJobRepository
{
}