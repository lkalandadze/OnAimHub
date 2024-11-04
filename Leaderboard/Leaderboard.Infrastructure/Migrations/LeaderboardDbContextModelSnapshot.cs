﻿// <auto-generated />
using System;
using Leaderboard.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    [DbContext(typeof(LeaderboardDbContext))]
    partial class LeaderboardDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Leaderboard.Domain.Entities.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardProgress", b =>
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

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("PlayerUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardRecordId");

                    b.ToTable("LeaderboardProgresses");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("AnnouncementDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsGenerated")
                        .HasColumnType("boolean");

                    b.Property<int?>("LeaderboardTemplateId")
                        .HasColumnType("integer");

                    b.Property<int>("LeaderboardType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardTemplateId");

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

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EndRank")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LeaderboardRecordId")
                        .HasColumnType("integer");

                    b.Property<string>("PrizeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StartRank")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardRecordId");

                    b.HasIndex("PrizeId");

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

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LeaderboardTemplateId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RepeatType")
                        .HasColumnType("integer");

                    b.Property<int?>("RepeatValue")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("SpecificDate")
                        .HasColumnType("date");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("interval");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardTemplateId");

                    b.ToTable("LeaderboardSchedules");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnnounceIn")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EndIn")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StartIn")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("LeaderboardTemplate");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardTemplatePrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EndRank")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LeaderboardTemplateId")
                        .HasColumnType("integer");

                    b.Property<string>("PrizeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StartRank")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardTemplateId");

                    b.HasIndex("PrizeId");

                    b.ToTable("LeaderboardTemplatePrize");
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

            modelBuilder.Entity("Leaderboard.Domain.Entities.Prize", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Prize");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardProgress", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardRecord", "LeaderboardRecord")
                        .WithMany("LeaderboardProgresses")
                        .HasForeignKey("LeaderboardRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardRecord");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecord", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardTemplate", "LeaderboardTemplate")
                        .WithMany()
                        .HasForeignKey("LeaderboardTemplateId");

                    b.Navigation("LeaderboardTemplate");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecordPrize", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardRecord", "LeaderboardRecord")
                        .WithMany("LeaderboardRecordPrizes")
                        .HasForeignKey("LeaderboardRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Leaderboard.Domain.Entities.Prize", "Prize")
                        .WithMany()
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardRecord");

                    b.Navigation("Prize");
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
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardTemplate", "LeaderboardTemplate")
                        .WithMany()
                        .HasForeignKey("LeaderboardTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardTemplate");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardTemplatePrize", b =>
                {
                    b.HasOne("Leaderboard.Domain.Entities.LeaderboardTemplate", "LeaderboardTemplate")
                        .WithMany("LeaderboardTemplatePrizes")
                        .HasForeignKey("LeaderboardTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Leaderboard.Domain.Entities.Prize", "Prize")
                        .WithMany()
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaderboardTemplate");

                    b.Navigation("Prize");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardRecord", b =>
                {
                    b.Navigation("LeaderboardProgresses");

                    b.Navigation("LeaderboardRecordPrizes");
                });

            modelBuilder.Entity("Leaderboard.Domain.Entities.LeaderboardTemplate", b =>
                {
                    b.Navigation("LeaderboardTemplatePrizes");
                });
#pragma warning restore 612, 618
        }
    }
}
