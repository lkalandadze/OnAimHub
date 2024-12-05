using GameLib.Infrastructure.Repositories;
using TestWheel.Domain.Abstractions.Repository;
using TestWheel.Domain.Entities;
using TestWheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;

public class TestWheelPrizeRepository(TestWheelConfigDbContext context) : BaseRepository<TestWheelConfigDbContext, TestWheelPrize>(context), ITestWheelPrizeRepository
{
}