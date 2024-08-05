using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using Shared.Infrastructure.DataAccess;

namespace Shared.Infrastructure.Repositories;

public class SegmentRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, Segment>(context), ISegmentRepository
{
}