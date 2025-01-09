using GameLib.Infrastructure.Repositories;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using PenaltyKicks.Infrastructure.DataAccess;

namespace PenaltyKicks.Infrastructure.Repositories;

public class PenaltyGameRepository(PenaltyConfigDbContext context) : BaseRepository<PenaltyConfigDbContext, PenaltyGame>(context), IPenaltyGameRepository
{
}