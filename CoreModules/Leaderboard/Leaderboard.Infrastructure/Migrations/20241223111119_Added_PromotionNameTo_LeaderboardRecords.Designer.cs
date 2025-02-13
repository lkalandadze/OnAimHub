﻿// <auto-generated />
using System;
using Leaderboard.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    [DbContext(typeof(LeaderboardDbContext))]
    [Migration("20241223111119_Added_PromotionNameTo_LeaderboardRecords")]
    partial class Added_PromotionNameTo_LeaderboardRecords
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("AnnouncementDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EventType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsGenerated")
                        .HasColumnType("boolean");

                    b.Property<int>("PromotionId")
                        .HasColumnType("integer");

                    b.Property<string>("PromotionName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TemplateId")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LeaderboardRecords");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecordPrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<string>("CoinId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EndRank")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LeaderboardRecordId")
                        .HasColumnType("integer");

                    b.Property<int>("StartRank")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardRecordId");

                    b.ToTable("LeaderboardRecordPrizes");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int>("LeaderboardRecordId")
                        .HasColumnType("integer");

                    b.Property<int>("Placement")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("PlayerUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardRecordId");

                    b.ToTable("LeaderboardResults");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnnouncementDurationHours")
                        .HasColumnType("integer");

                    b.Property<int>("CreationHours")
                        .HasColumnType("integer");

                    b.Property<int>("EndDurationHours")
                        .HasColumnType("integer");

                    b.Property<int>("LeaderboardRecordId")
                        .HasColumnType("integer");

                    b.Property<int>("RepeatType")
                        .HasColumnType("integer");

                    b.Property<int?>("RepeatValue")
                        .HasColumnType("integer");

                    b.Property<int>("StartDurationHours")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardRecordId")
                        .IsUnique();

                    b.ToTable("LeaderboardSchedules");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecordPrize", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardRecord", "LeaderboardRecord")
                        .WithMany("LeaderboardRecordPrizes")
                        .HasForeignKey("LeaderboardRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardRecord");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardResult", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardRecord", "LeaderboardRecord")
                        .WithMany()
                        .HasForeignKey("LeaderboardRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardRecord");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardSchedule", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardRecord", "LeaderboardRecord")
                        .WithOne("LeaderboardSchedule")
                        .HasForeignKey("Leaderboard.Domain.Entities.LeaderboardSchedule", "LeaderboardRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardRecord");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecord", b =>
                {
                    b.Navigation("LeaderboardRecordPrizes");

                    b.Navigation("LeaderboardSchedule")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
