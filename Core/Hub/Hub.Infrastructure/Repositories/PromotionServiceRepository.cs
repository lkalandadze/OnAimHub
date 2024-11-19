using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionServiceRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionService>(context), IPromotionServiceRepository
{
}