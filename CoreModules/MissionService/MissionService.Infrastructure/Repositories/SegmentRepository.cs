using MissionService.Domain.Abstractions.Repository;
using MissionService.Infrastructure.DataAccess;

namespace MissionService.Infrastructure.Repositories;

public class SegmentRepository(MissionDbContext context) : BaseRepository<MissionDbContext, Domain.Entities.Segment>(context), ISegmentRepository
{
}
