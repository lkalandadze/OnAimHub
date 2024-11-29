using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CoinType = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsulLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    ServiceName = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Registration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsulLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HubSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    ExecutionTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    LastExecutedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    IntervalInDays = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerLogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerLogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    ReferrerId = table.Column<int>(type: "integer", nullable: true),
                    HasPlayed = table.Column<bool>(type: "boolean", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    RegistredOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastVisitedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSegmentActTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSegmentActTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PriorityLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    AccessTokenExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawEndpointTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    EndpointContent = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawEndpointTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrizeTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrizeTypes_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerBans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    DateBanned = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpireDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsPermanent = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBans_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Log = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    PlayerLogTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerLogs_PlayerLogTypes_PlayerLogTypeId",
                        column: x => x.PlayerLogTypeId,
                        principalTable: "PlayerLogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerLogs_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProgresses_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerProgresses_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProgressHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProgressHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProgressHistories_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerProgressHistories_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferralDistributions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferrerId = table.Column<int>(type: "integer", nullable: false),
                    ReferralId = table.Column<int>(type: "integer", nullable: false),
                    ReferrerPrizeValue = table.Column<int>(type: "integer", nullable: false),
                    ReferrerPrizeId = table.Column<string>(type: "text", nullable: true),
                    ReferrerPrizeCurrencyId = table.Column<string>(type: "text", nullable: true),
                    ReferralPrizeValue = table.Column<int>(type: "integer", nullable: false),
                    ReferralPrizeId = table.Column<string>(type: "text", nullable: true),
                    ReferralPrizeCurrencyId = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralDistributions_Currencies_ReferralPrizeCurrencyId",
                        column: x => x.ReferralPrizeCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReferralDistributions_Currencies_ReferrerPrizeCurrencyId",
                        column: x => x.ReferrerPrizeCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReferralDistributions_Players_ReferralId",
                        column: x => x.ReferralId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralDistributions_Players_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    PromotionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBalances_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerBalances_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerBalances_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CoinType = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    CoinTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionCoins_CoinTemplates_CoinTemplateId",
                        column: x => x.CoinTemplateId,
                        principalTable: "CoinTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionCoins_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Service = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionServices_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsClaimed = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rewards_RewardSource_SourceId",
                        column: x => x.SourceId,
                        principalTable: "RewardSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBlockedSegmentMappings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    SegmentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBlockedSegmentMappings", x => new { x.PlayerId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_PlayerBlockedSegmentMappings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerBlockedSegmentMappings_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSegmentActs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalPlayers = table.Column<int>(type: "integer", nullable: false),
                    ByUserId = table.Column<int>(type: "integer", nullable: true),
                    IsBulk = table.Column<bool>(type: "boolean", nullable: false),
                    ActionId = table.Column<int>(type: "integer", nullable: true),
                    SegmentId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSegmentActs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSegmentActs_PlayerSegmentActTypes_ActionId",
                        column: x => x.ActionId,
                        principalTable: "PlayerSegmentActTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerSegmentActs_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerSegmentMappings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    SegmentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSegmentMappings", x => new { x.PlayerId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_PlayerSegmentMappings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSegmentMappings_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionSegmentMappings",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    SegmentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionSegmentMappings", x => new { x.PromotionId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_PromotionSegmentMappings_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionSegmentMappings_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: true),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    FromAccountId = table.Column<int>(type: "integer", nullable: false),
                    ToAccountId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    PromotionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountTypes_FromAccountId",
                        column: x => x.FromAccountId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountTypes_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TransactionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    EndpointContent = table.Column<string>(type: "text", nullable: true),
                    WithdrawEndpointTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                        column: x => x.WithdrawEndpointTemplateId,
                        principalTable: "WithdrawEndpointTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RewardPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    RewardId = table.Column<int>(type: "integer", nullable: false),
                    PrizeTypeId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardPrizes_PrizeTypes_PrizeTypeId",
                        column: x => x.PrizeTypeId,
                        principalTable: "PrizeTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RewardPrizes_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSegmentActHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    PlayerSegmentActId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSegmentActHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSegmentActHistories_PlayerSegmentActs_PlayerSegmentAc~",
                        column: x => x.PlayerSegmentActId,
                        principalTable: "PlayerSegmentActs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSegmentActHistories_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionCoinTemplateMappings",
                columns: table => new
                {
                    CoinTemplateId = table.Column<int>(type: "integer", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionCoinTemplateMappings", x => new { x.CoinTemplateId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_CoinTemplates_CoinTempla~",
                        column: x => x.CoinTemplateId,
                        principalTable: "CoinTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptions_Withdraw~",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionGroupMappings",
                columns: table => new
                {
                    WithdrawOptionGroupId = table.Column<int>(type: "integer", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionGroupMappings", x => new { x.WithdrawOptionGroupId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupMappings_WithdrawOptionGroups_WithdrawOp~",
                        column: x => x.WithdrawOptionGroupId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupMappings_WithdrawOptions_WithdrawOptionId",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionPromotionCoinMappings",
                columns: table => new
                {
                    PromotionCoinId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionPromotionCoinMappings", x => new { x.PromotionCoinId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionPromotionCoinMappings_PromotionCoins_Promotio~",
                        column: x => x.PromotionCoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionPromotionCoinMappings_WithdrawOptions_Withdra~",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBalances_CurrencyId",
                table: "PlayerBalances",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBalances_PlayerId",
                table: "PlayerBalances",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBalances_PromotionId",
                table: "PlayerBalances",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_PlayerId",
                table: "PlayerBans",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBlockedSegmentMappings_SegmentId",
                table: "PlayerBlockedSegmentMappings",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLogs_PlayerId",
                table: "PlayerLogs",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLogs_PlayerLogTypeId",
                table: "PlayerLogs",
                column: "PlayerLogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgresses_CurrencyId",
                table: "PlayerProgresses",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgresses_PlayerId",
                table: "PlayerProgresses",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgressHistories_CurrencyId",
                table: "PlayerProgressHistories",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgressHistories_PlayerId",
                table: "PlayerProgressHistories",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSegmentActHistories_PlayerId",
                table: "PlayerSegmentActHistories",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSegmentActHistories_PlayerSegmentActId",
                table: "PlayerSegmentActHistories",
                column: "PlayerSegmentActId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSegmentActs_ActionId",
                table: "PlayerSegmentActs",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSegmentActs_SegmentId",
                table: "PlayerSegmentActs",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSegmentMappings_SegmentId",
                table: "PlayerSegmentMappings",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeTypes_CurrencyId",
                table: "PrizeTypes",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoins_CoinTemplateId",
                table: "Coins",
                column: "CoinTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoins_PromotionId",
                table: "Coins",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionSegmentMappings_SegmentId",
                table: "PromotionSegmentMappings",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_PromotionId",
                table: "PromotionServices",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralDistributions_ReferralId",
                table: "ReferralDistributions",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralDistributions_ReferralPrizeCurrencyId",
                table: "ReferralDistributions",
                column: "ReferralPrizeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralDistributions_ReferrerId",
                table: "ReferralDistributions",
                column: "ReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralDistributions_ReferrerPrizeCurrencyId",
                table: "ReferralDistributions",
                column: "ReferrerPrizeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPrizes_PrizeTypeId",
                table: "RewardPrizes",
                column: "PrizeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPrizes_RewardId",
                table: "RewardPrizes",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_PlayerId",
                table: "Rewards",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_SourceId",
                table: "Rewards",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyId",
                table: "Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromAccountId",
                table: "Transactions",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GameId",
                table: "Transactions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PlayerId",
                table: "Transactions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PromotionId",
                table: "Transactions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatusId",
                table: "Transactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToAccountId",
                table: "Transactions",
                column: "ToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TypeId",
                table: "Transactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionId",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionGroupMappings_WithdrawOptionId",
                table: "WithdrawOptionGroupMappings",
                column: "WithdrawOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionPromotionCoinMappings_WithdrawOptionId",
                table: "WithdrawOptionPromotionCoinMappings",
                column: "WithdrawOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsulLogs");

            migrationBuilder.DropTable(
                name: "HubSettings");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "PlayerBalances");

            migrationBuilder.DropTable(
                name: "PlayerBans");

            migrationBuilder.DropTable(
                name: "PlayerBlockedSegmentMappings");

            migrationBuilder.DropTable(
                name: "PlayerLogs");

            migrationBuilder.DropTable(
                name: "PlayerProgresses");

            migrationBuilder.DropTable(
                name: "PlayerProgressHistories");

            migrationBuilder.DropTable(
                name: "PlayerSegmentActHistories");

            migrationBuilder.DropTable(
                name: "PlayerSegmentMappings");

            migrationBuilder.DropTable(
                name: "PromotionSegmentMappings");

            migrationBuilder.DropTable(
                name: "PromotionServices");

            migrationBuilder.DropTable(
                name: "ReferralDistributions");

            migrationBuilder.DropTable(
                name: "RewardPrizes");

            migrationBuilder.DropTable(
                name: "TokenRecords");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "WithdrawOptionCoinTemplateMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionGroupMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionPromotionCoinMappings");

            migrationBuilder.DropTable(
                name: "PlayerLogTypes");

            migrationBuilder.DropTable(
                name: "PlayerSegmentActs");

            migrationBuilder.DropTable(
                name: "PrizeTypes");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "TransactionStatuses");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "WithdrawOptionGroups");

            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "WithdrawOptions");

            migrationBuilder.DropTable(
                name: "PlayerSegmentActTypes");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "RewardSource");

            migrationBuilder.DropTable(
                name: "CoinTemplates");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "WithdrawEndpointTemplates");
        }
    }
}
