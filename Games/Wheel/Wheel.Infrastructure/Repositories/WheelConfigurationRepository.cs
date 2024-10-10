using GameLib.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;


public class WheelConfigurationRepository : BaseRepository<WheelConfigDbContext, WheelConfiguration>, IWheelConfigurationRepository
{
    private readonly WheelConfigDbContext _context;

    public WheelConfigurationRepository(WheelConfigDbContext context) : base(context)
    {
        _context = context;
    }

    public DbContext GetDbContext()
    {
        return _context;
    }
}