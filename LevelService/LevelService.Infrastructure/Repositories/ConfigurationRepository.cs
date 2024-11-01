using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using LevelService.Infrastructure.DataAccess;

namespace LevelService.Infrastructure.Repositories;

public class ConfigurationRepository(LevelDbContext context) : BaseRepository<LevelDbContext, Configuration>(context), IConfigurationRepository
{
}