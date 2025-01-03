﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wheel.Infrastructure.DataAccess;

#nullable disable

namespace Wheel.Infrastructure.Migrations
{
    [DbContext(typeof(WheelConfigDbContext))]
    [Migration("20241114044716_DeleteSegments")]
    partial class DeleteSegments
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GameLib.Domain.Entities.Coin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.GameSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GameSettings");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.Price", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("CoinId")
                        .HasColumnType("text");

                    b.Property<decimal>("Multiplier")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.Property<int?>("WheelConfigurationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.HasIndex("WheelConfigurationId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.PrizeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CoinId")
                        .HasColumnType("text");

                    b.Property<bool>("IsMultiplied")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.ToTable("PrizeTypes");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.JackpotPrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PrizeGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("PrizeTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("Probability")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PrizeGroupId");

                    b.HasIndex("PrizeTypeId");

                    b.ToTable("JackpotPrizes");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.JackpotPrizeGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("integer");

                    b.Property<int?>("NextPrizeIndex")
                        .HasColumnType("integer");

                    b.Property<List<int>>("Sequence")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.HasKey("Id");

                    b.ToTable("JackpotPrizeGroups");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("NextPrizeIndex")
                        .HasColumnType("integer");

                    b.Property<List<int>>("Sequence")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.WheelConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GameConfigurations");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.WheelPrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PrizeGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("PrizeTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("Probability")
                        .HasColumnType("integer");

                    b.Property<int?>("RoundId")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<int?>("WheelIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PrizeGroupId");

                    b.HasIndex("PrizeTypeId");

                    b.ToTable("WheelPrizes");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.Price", b =>
                {
                    b.HasOne("GameLib.Domain.Entities.Coin", "Coin")
                        .WithMany("Prices")
                        .HasForeignKey("CoinId");

                    b.HasOne("Wheel.Domain.Entities.WheelConfiguration", null)
                        .WithMany("Prices")
                        .HasForeignKey("WheelConfigurationId");

                    b.Navigation("Coin");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.PrizeType", b =>
                {
                    b.HasOne("GameLib.Domain.Entities.Coin", "Coin")
                        .WithMany("PrizeTypes")
                        .HasForeignKey("CoinId");

                    b.Navigation("Coin");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.JackpotPrize", b =>
                {
                    b.HasOne("Wheel.Domain.Entities.JackpotPrizeGroup", "PrizeGroup")
                        .WithMany("Prizes")
                        .HasForeignKey("PrizeGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLib.Domain.Entities.PrizeType", "PrizeType")
                        .WithMany()
                        .HasForeignKey("PrizeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrizeGroup");

                    b.Navigation("PrizeType");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.Round", b =>
                {
                    b.HasOne("Wheel.Domain.Entities.WheelConfiguration", "Configuration")
                        .WithMany("Rounds")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Configuration");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.WheelPrize", b =>
                {
                    b.HasOne("Wheel.Domain.Entities.Round", "PrizeGroup")
                        .WithMany("Prizes")
                        .HasForeignKey("PrizeGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLib.Domain.Entities.PrizeType", "PrizeType")
                        .WithMany()
                        .HasForeignKey("PrizeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrizeGroup");

                    b.Navigation("PrizeType");
                });

            modelBuilder.Entity("GameLib.Domain.Entities.Coin", b =>
                {
                    b.Navigation("Prices");

                    b.Navigation("PrizeTypes");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.JackpotPrizeGroup", b =>
                {
                    b.Navigation("Prizes");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.Round", b =>
                {
                    b.Navigation("Prizes");
                });

            modelBuilder.Entity("Wheel.Domain.Entities.WheelConfiguration", b =>
                {
                    b.Navigation("Prices");

                    b.Navigation("Rounds");
                });
#pragma warning restore 612, 618
        }
    }
}
