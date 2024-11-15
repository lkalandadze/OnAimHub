using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class CoinTemplateRepository(HubDbContext context) : BaseRepository<HubDbContext, CoinTemplate>(context), ICoinTemplateRepository
{
}