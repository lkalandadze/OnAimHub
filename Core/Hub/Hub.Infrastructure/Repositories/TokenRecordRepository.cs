using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class TokenRecordRepository(HubDbContext context) : BaseRepository<HubDbContext, TokenRecord>(context), ITokenRecordRepository
{
}
