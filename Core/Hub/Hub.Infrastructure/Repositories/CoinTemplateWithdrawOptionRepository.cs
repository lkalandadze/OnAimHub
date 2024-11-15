using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class CoinTemplateWithdrawOptionRepository(HubDbContext context) : BaseRepository<HubDbContext, CoinTemplateWithdrawOption>(context), ICoinTemplateWithdrawOptionRepository
{
}