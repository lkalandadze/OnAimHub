using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;

namespace GameLib.Infrastructure.Repositories;

public class SegmentRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Segment>(context), ISegmentRepository
{
}