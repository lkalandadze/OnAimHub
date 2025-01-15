using GameLib.Infrastructure.Repositories;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using PenaltyKicks.Infrastructure.DataAccess;

namespace PenaltyKicks.Infrastructure.Repositories;

public class PenaltyPrizeGroupRepository(PenaltyConfigDbContext context) : BaseRepository<PenaltyConfigDbContext, PenaltyPrizeGroup>(context), IPenaltyPrizeGroupRepository
{
}