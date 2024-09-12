using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class ConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Configuration>(context), IConfigurationRepository
{
}