using MissionService.Domain.Abstractions.Repository;
using MissionService.Domain.Entities.DbEnums;
using MissionService.Infrastructure.DataAccess;

namespace MissionService.Infrastructure.Repositories;

public class PrizeTypeRepository(MissionDbContext context) : BaseRepository<MissionDbContext, PrizeType>(context), IPrizeTypeRepository
{
}