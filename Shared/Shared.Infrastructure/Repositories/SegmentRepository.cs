using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class SegmentRepository(GameConfigDbContext context) : BaseRepository<GameConfigDbContext, Segment>(context), ISegmentRepository
{
}