﻿// <auto-generated />
using System;
using LevelService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LevelService.Infrastructure.Migrations
{
    [DbContext(typeof(LevelDbContext))]
    partial class LevelDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LevelService.Domain.Entities.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CurrencyId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("ExperienceToGrant")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int?>("StageId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("StageId");

                    b.ToTable("Configuration", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.DbEnums.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currencies", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.DbEnums.PrizeType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("CurrencyId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("PrizeTypes", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExperienceToArchieve")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int?>("StageId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StageId");

                    b.ToTable("Levels", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.LevelPrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int?>("LevelId")
                        .HasColumnType("integer");

                    b.Property<int>("PrizeDeliveryType")
                        .HasColumnType("integer");

                    b.Property<string>("PrizeTypeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RankId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LevelId");

                    b.HasIndex("PrizeTypeId");

                    b.ToTable("LevelPrizes", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Experience")
                        .HasColumnType("numeric");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.PlayerReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("DateClaimed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LevelPrizeId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int>("RewardStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LevelPrizeId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerReward", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Stage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DateFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DateTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsExpirable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Stages", (string)null);
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Configuration", b =>
                {
                    b.HasOne("LevelService.Domain.Entities.DbEnums.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LevelService.Domain.Entities.Stage", null)
                        .WithMany("Configurations")
                        .HasForeignKey("StageId");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.DbEnums.PrizeType", b =>
                {
                    b.HasOne("LevelService.Domain.Entities.DbEnums.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Level", b =>
                {
                    b.HasOne("LevelService.Domain.Entities.Stage", null)
                        .WithMany("Levels")
                        .HasForeignKey("StageId");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.LevelPrize", b =>
                {
                    b.HasOne("LevelService.Domain.Entities.Level", null)
                        .WithMany("LevelPrizes")
                        .HasForeignKey("LevelId");

                    b.HasOne("LevelService.Domain.Entities.DbEnums.PrizeType", "PrizeType")
                        .WithMany()
                        .HasForeignKey("PrizeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrizeType");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.PlayerReward", b =>
                {
                    b.HasOne("LevelService.Domain.Entities.LevelPrize", "LevelPrize")
                        .WithMany()
                        .HasForeignKey("LevelPrizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LevelService.Domain.Entities.Player", "Player")
                        .WithMany("PlayerRewards")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LevelPrize");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Level", b =>
                {
                    b.Navigation("LevelPrizes");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Player", b =>
                {
                    b.Navigation("PlayerRewards");
                });

            modelBuilder.Entity("LevelService.Domain.Entities.Stage", b =>
                {
                    b.Navigation("Configurations");

                    b.Navigation("Levels");
                });
#pragma warning restore 612, 618
        }
    }
}
