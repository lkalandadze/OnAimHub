using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class ConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Configuration>(context), IConfigurationRepository
{
}