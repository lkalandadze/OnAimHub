using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Templates;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class PromotionViewTemplateRepository(HubDbContext context) : BaseRepository<HubDbContext, PromotionViewTemplate>(context), IPromotionViewTemplateRepository
{
}