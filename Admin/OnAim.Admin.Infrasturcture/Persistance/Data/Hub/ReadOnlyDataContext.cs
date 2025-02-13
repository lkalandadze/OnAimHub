﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Enum;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.Hub;

public class ReadOnlyDataContext : DbContext
{
    public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options)
        : base(options)
    {
    }

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
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<RewardPrize> RewardPrizes { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionView> PromotionViews { get; set; }
    public DbSet<Coin> Coins { get; set; }
    public DbSet<WithdrawOption> WithdrawOptions { get; set; }
    public DbSet<WithdrawOptionEndpoint> WithdrawOptionEndpoints { get; set; }
    public DbSet<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }
    public DbSet<Service> Services { get; set; }

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

        modelBuilder.Entity<AssetCoin>().Property(ac => ac.Value)
               .HasColumnName($"{nameof(AssetCoin)}_{nameof(AssetCoin.Value)}");

        modelBuilder.Entity<AssetCoin>().HasDiscriminator<CoinType>(nameof(CoinType))
               .HasValue<AssetCoin>(CoinType.Asset);

        modelBuilder.Entity<Coin>().HasDiscriminator<Domain.HubEntities.Enum.CoinType>(nameof(Domain.HubEntities.Enum.CoinType))
             .HasValue<Coin>(Domain.HubEntities.Enum.CoinType.Default) // Default value for the base type
             .HasValue<InCoin>(Domain.HubEntities.Enum.CoinType.In)
             .HasValue<OutCoin>(Domain.HubEntities.Enum.CoinType.Out)
             .HasValue<InternalCoin>(Domain.HubEntities.Enum.CoinType.Internal)
             .HasValue<AssetCoin>(Domain.HubEntities.Enum.CoinType.Asset);

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

        modelBuilder.Entity<WithdrawOptionGroup>().HasMany(w => w.OutCoins)
              .WithMany(oc => oc.WithdrawOptionGroups)
              .UsingEntity<Dictionary<string, object>>(
                   $"{nameof(Coin)}{nameof(WithdrawOptionGroup)}Mappings",
                   join => join.HasOne<OutCoin>()
                               .WithMany()
                               .HasForeignKey($"{nameof(Coin)}{nameof(Coin.Id)}")
                               .OnDelete(DeleteBehavior.Cascade),
                   join => join.HasOne<WithdrawOptionGroup>()
                               .WithMany()
                               .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                               .OnDelete(DeleteBehavior.Cascade)
               );

        modelBuilder.Entity<WithdrawOption>().HasMany(w => w.WithdrawOptionGroups)
               .WithMany(g => g.WithdrawOptions)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(WithdrawOptionGroup)}Mappings",
                    j => j.HasOne<WithdrawOptionGroup>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<WithdrawOption>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                        .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship between WithdrawOption and PromotionCoins
        modelBuilder.Entity<WithdrawOption>().HasMany(w => w.OutCoins)
          .WithMany(c => c.WithdrawOptions)
          .UsingEntity<Dictionary<string, object>>(
               $"{nameof(Coin)}{nameof(WithdrawOption)}Mappings",
               j => j.HasOne<OutCoin>()
                   .WithMany()
                   .HasForeignKey($"{nameof(Coin)}{nameof(Coin.Id)}")
                   .OnDelete(DeleteBehavior.Cascade),
               j => j.HasOne<WithdrawOption>()
                   .WithMany()
                   .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                   .OnDelete(DeleteBehavior.Cascade)
          );

        modelBuilder.Entity<Service>().HasMany(s => s.Promotions)
       .WithMany(p => p.Services)
       .UsingEntity<Dictionary<string, object>>(
            $"{nameof(Promotion)}{nameof(Service)}Mappings",
            j => j.HasOne<Promotion>()
                  .WithMany()
                  .HasForeignKey($"{nameof(Promotion)}{nameof(Promotion.Id)}")
                  .OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Service>()
                  .WithMany()
                  .HasForeignKey($"{nameof(Service)}{nameof(Service.Id)}")
                  .OnDelete(DeleteBehavior.Cascade)
        );

        base.OnModelCreating(modelBuilder);
    }
}
