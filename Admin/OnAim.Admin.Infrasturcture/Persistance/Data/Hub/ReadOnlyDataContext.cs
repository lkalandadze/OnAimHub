using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.Hub;

public class ReadOnlyDataContext : DbContext
{
    public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options)
        : base(options)
    {
    }

    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerBalance> PlayerBalances { get; set; }
    public DbSet<PlayerProgress> PlayerProgresses { get; set; }
    public DbSet<PlayerProgressHistory> PlayerProgressHistories { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<PlayerSegmentAct> PlayerSegmentActs { get; set; }
    public DbSet<PlayerSegmentActHistory> PlayerSegmentActHistories { get; set; }
    public DbSet<PlayerSegmentActType> PlayerSegmentActTypes { get; set; }
    public DbSet<PlayerLog> PlayerLogs { get; set; }
    public DbSet<PlayerLogType> PlayerLogTypes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<AccountType> AccountTypes { get; set; }
    public DbSet<ReferralDistribution> ReferralDistributions { get; set; }
    public DbSet<PlayerBan> PlayerBans { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionService> PromotionServices { get; set; }
    public DbSet<OnAim.Admin.Domain.HubEntities.PromotionCoin> PromotionCoins { get; set; }
    public DbSet<OnAim.Admin.Domain.HubEntities.WithdrawOption> WithdrawOptions { get; set; }
    public DbSet<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }

    public IQueryable<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>().AsNoTracking();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OnAim.Admin.Domain.HubEntities.WithdrawOption>().HasMany(w => w.WithdrawOptionGroups)
               .WithMany(g => g.WithdrawOptions)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(OnAim.Admin.Domain.HubEntities.WithdrawOptionGroup)}Mappings",
                    j => j.HasOne<WithdrawOptionGroup>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<WithdrawOption>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                        .OnDelete(DeleteBehavior.Cascade)
               );

        // Many-to-Many Relationship between WithdrawOption and CoinTemplates
        //modelBuilder.Entity<WithdrawOption>().HasMany(w => w.CoinTemplates)
        //    .WithMany(c => c.WithdrawOptions)
        //    .UsingEntity<Dictionary<string, object>>(
        //        $"{nameof(WithdrawOption)}{nameof(CoinTemplate)}Mappings",
        //        j => j.HasOne<CoinTemplate>()
        //            .WithMany()
        //            .HasForeignKey($"{nameof(CoinTemplate)}{nameof(CoinTemplate.CoinTemplateId)}")
        //            .OnDelete(DeleteBehavior.Cascade),
        //        j => j.HasOne<WithdrawOption>()
        //            .WithMany()
        //            .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
        //            .OnDelete(DeleteBehavior.Cascade)
        //    );


        // Many-to-Many Relationship between WithdrawOption and PromotionCoins
        //modelBuilder.Entity<WithdrawOption>().HasMany(w => w.PromotionCoins)
        //  .WithMany(c => c.WithdrawOptions)
        //  .UsingEntity<Dictionary<string, object>>(
        //       $"{nameof(WithdrawOption)}{nameof(PromotionCoin)}Mappings",
        //       j => j.HasOne<PromotionCoin>()
        //           .WithMany()
        //           .HasForeignKey($"{nameof(PromotionCoin)}{nameof(PromotionCoin.Id)}")
        //           .OnDelete(DeleteBehavior.Cascade),
        //       j => j.HasOne<WithdrawOption>()
        //           .WithMany()
        //           .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
        //           .OnDelete(DeleteBehavior.Cascade)
        //  );

        // Many-to-Many Relationship between Segment and Players
        modelBuilder.Entity<Segment>().HasMany(s => s.Players)
               .WithMany(p => p.Segments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Player)}{nameof(Segment)}Mappings",
                    j => j.HasOne<Player>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Player)}{nameof(Player.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship for BlockedPlayers
        modelBuilder.Entity<Segment>().HasMany(s => s.BlockedPlayers)
               .WithMany(p => p.BlockedSegments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Player)}Blocked{nameof(Segment)}Mappings",
                    j => j.HasOne<Player>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Player)}{nameof(Player.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship between Segments and Promotions
        modelBuilder.Entity<Segment>().HasMany(s => s.Promotions)
               .WithMany(p => p.Segments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Promotion)}{nameof(Segment)}Mappings",
                    j => j.HasOne<Promotion>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Promotion)}{nameof(Promotion.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );

        base.OnModelCreating(modelBuilder);
    }
}
