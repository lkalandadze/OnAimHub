using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionRepository(HubDbContext context) : BaseRepository<HubDbContext, Promotion>(context), IPromotionRepository
{
}