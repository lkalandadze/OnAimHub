using GameLib.Infrastructure.Repositories;
using TestWheel.Domain.Abstractions.Repository;
using TestWheel.Domain.Entities;
using TestWheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;

public class RoundRepository(TestWheelConfigDbContext context) : BaseRepository<TestWheelConfigDbContext, Round>(context), IRoundRepository
{
}