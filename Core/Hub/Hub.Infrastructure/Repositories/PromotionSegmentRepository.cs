using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionSegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionSegment>(context), IPromotionSegmentRepository
{
}