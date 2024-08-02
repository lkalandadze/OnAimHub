using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class ConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Base.Configuration>(context), IConfigurationRepository
{
}