﻿using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Wheel.Domain.Entities;

namespace Wheel.Infrastructure.DataAccess;

public class WheelConfigDbContext : SharedGameConfigDbContext
{
    public WheelConfigDbContext(DbContextOptions<WheelConfigDbContext> options)
        : base(options)
    {
    }

    public DbSet<WheelPrizeGroup> WheelPrizeGroups { get; set; }
    public DbSet<WheelPrize> WheelPrizes { get; set; }

    public DbSet<JackpotPrizeGroup> JackpotPrizeGroups { get; set; }
    public DbSet<JackpotPrize> JackpotPrizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}