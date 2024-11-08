using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class LevelRepository(HubDbContext context) : BaseRepository<HubDbContext, Level>(context), ILevelRepository
{
}