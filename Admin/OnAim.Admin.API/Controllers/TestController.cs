using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.HubEntities.Enum;
using OnAim.Admin.Domain.HubEntities.Models;

namespace OnAim.Admin.API.Controllers;

public class TestController : ApiControllerBase
{
    private IPromotionService _promotionService;
    public TestController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpPost]
    public async Task<IActionResult> Test()
    {
        var aggregList = new List<AggregationConfiguration>();

       

        var coinIn = new CreateInCoinModel
        {
            Name = "WinterCoin",
            Description = "A special coin for winter events.",
            ImageUrl = "",
            TemplateId = "coin-template-001",
            //Configurations = coinInAggregation
        };

        var assetCoin = new CreateAssetCoinModel
        {
            Name = "GoldCoin",
            Description = "Gold asset coin for premium users.",
            ImageUrl = "",
            Value = 1000,
            TemplateId = "asset-template-001"
        };

        var coins = new List<CreateCoinModel>
        {
            coinIn,
            assetCoin
        };

        var correlation = Guid.NewGuid();

        var prom = new CreatePromotionCommandDto
        {
            Title = "Super Winter Promo",
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(1),
            Description = "A promotion to engage users during the winter season.",
            CorrelationId = correlation,
            TemplateId = "promo-template-001",
            SegmentIds = new List<string> { "default" },
            Coins = coins,
            ServiceIds = new List<int> { 1 }
        };

        var promCreationResult = await _promotionService.CreatePromotionAsync(prom);

        string assetCoinId = $"{promCreationResult.PromotionId}_{assetCoin.Name}";
        string coinInId = $"{promCreationResult.PromotionId}_{coinIn.Name}";

        //move up and set promoid in coin id later when you have it returend
        var coinInAggregation = new List<AggregationConfiguration>
        {
            new AggregationConfiguration(
               name: "Weekly Points Aggregation",
               description: "Aggregates points on a weekly basis.",
               eventProducer: "External",
               aggregationSubscriber: "Hub",
               filters: new List<Filter>
           {
                        new Filter("BetAmount", Operator.Equals, "10")
           },
               aggregationType: AggregationType.Sum,
               evaluationType: EvaluationType.SingleRule,
               pointEvaluationRules: new List<PointEvaluationRule>
               {
                            new PointEvaluationRule(50, 1),
               },
               selectionField: "BetAmount",
               expiration: DateTime.UtcNow.AddDays(7),
               promotionId: promCreationResult.PromotionId.ToString(),
               key: coinInId
           )
        };

        foreach (var item in coinInAggregation)
        {
            item.PromotionId = $"{promCreationResult.PromotionId}";
            aggregList.Add(item);
        }

        var leaderb = new List<CreateLeaderboardRecord>
            {
                    new CreateLeaderboardRecord
                    {
                        PromotionId = promCreationResult.PromotionId,
                        PromotionName = "Super Winter Promo",
                        Title = "Winter Leaderboard",
                        Description = "Track user scores during the winter promotion.",
                        EventType = EventType.External,
                        RepeatType = RepeatType.EveryNDays,
                        RepeatValue = 1,
                        AnnouncementDate = DateTimeOffset.UtcNow.AddDays(-1),
                        StartDate = DateTimeOffset.UtcNow,
                        EndDate = DateTimeOffset.UtcNow.AddDays(30),
                        Status = LeaderboardRecordStatus.Announced,
                        IsGenerated = true,
                        CorrelationId = correlation,
                        LeaderboardPrizes = new List<CreateLeaderboardRecordPrizeCommandItem>
                        {
                            new CreateLeaderboardRecordPrizeCommandItem
                            {
                                CoinId = assetCoin.Name,
                                StartRank = 1,
                                EndRank = 1,
                                Amount = 1000
                            },
                            new CreateLeaderboardRecordPrizeCommandItem
                            {
                                CoinId = coinIn.Name,
                                StartRank = 2,
                                EndRank = 5,
                                Amount = 500
                            }
                        },
                        AggregationConfigurations = null
                    }
            };

        var leadExternalId = await _promotionService.CreateLeaderboardRecordAsync(leaderb[0]);

        foreach (var item in leaderb)
        {
            item.AggregationConfigurations = new List<AggregationConfiguration>
                        {
                            new AggregationConfiguration(
                                name: "Score Aggregation",
                                description: "Aggregates scores based on user activity.",
                                eventProducer: "Hub",
                                aggregationSubscriber: "Leaderboard",
                                filters: new List<Filter>
                                {
                                    new Filter("gameName", Operator.Equals, "Wheel"),
                                    new Filter("eventType", Operator.Equals, "Bet"),
                                    new Filter("CoinId", Operator.Equals, coinInId),
                                },
                                aggregationType: AggregationType.Sum,
                                evaluationType: EvaluationType.Steps,
                                pointEvaluationRules: new List<PointEvaluationRule>
                                {
                                    new PointEvaluationRule(1, 10),
                                    new PointEvaluationRule(5, 50)
                                },
                                selectionField: "BetAmount",
                                expiration: DateTime.UtcNow.AddDays(7),
                                promotionId: Guid.NewGuid().ToString(),
                                key: $"{leadExternalId}"
                            )
                        };
            aggregList.Add(item.AggregationConfigurations[0]);
        } 

        await _promotionService.CreateAggregationConfiguration(aggregList);

        var game = new List<Contracts.Dtos.Promotion.GameConfigDto>
            {
                new Contracts.Dtos.Promotion.GameConfigDto
                {
                    GameName = "wheel",
                    GameConfiguration = new
                    {
                         Id = 0,
                         Name = "wheel configuration",
                         Value = 1,
                         IsActive = true,
                         PromotionId = promCreationResult.PromotionId,
                         CorrelationId = correlation,
                         FromTemplateId = "string",
                         Prices = new List<object>
                         {
                             new
                             {
                                 Id = 0,
                                 Value = 10,
                                 Multiplier = 2,
                                 CoinId = "string"
                             }
                         },
                         WheelPrizeGroups = new List<object>
                         {
                             new
                             {
                                 Id = 0,
                                 Sequence = new List<int> { 1, 2, 3 },
                                 NextPrizeIndex = 0,
                                 ConfigurationId = 0,
                                 Prizes = new List<object>
                                 {
                                     new
                                     {
                                         Id = 0,
                                         Value = 50,
                                         Probability = 50,
                                         CoinId = assetCoin.Name,
                                         PrizeGroupId = 0,
                                         Name = "Prize A",
                                         WheelIndex = 1
                                     },
                                     new
                                     {
                                         Id = 0,
                                         Value = 30,
                                         Probability = 30,
                                         CoinId = coinIn.Name,
                                         PrizeGroupId = 0,
                                         Name = "Prize B",
                                         WheelIndex = 2
                                     },
                                     new
                                     {
                                         Id = 0,
                                         Value = 20,
                                         Probability = 20,
                                         CoinId = assetCoin.Name,
                                         PrizeGroupId = 0,
                                         Name = "Prize C",
                                         WheelIndex = 3
                                     }
                                 },
                                 Name = "Wheel Prize Group 1"
                             }
                                 }
                             },
                },
                new Contracts.Dtos.Promotion.GameConfigDto
                {
                    GameName = "penaltykicks",
                    GameConfiguration = new
                    {
                        Id = 0,
                        Name = "For Promotion 1",
                        Description = "Main configuration of promotion 1",
                        Value = 1,
                        IsActive = true,
                        PromotionId = 1111,
                        CorrelationId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                        Prices = new List<object>
                        {
                            new { Id = 0, Value = 5, Multiplier = 2, CoinId = assetCoinId },
                            new { Id = 0, Value = 10, Multiplier = 3, CoinId = assetCoinId },
                            new { Id = 0, Value = 15, Multiplier = 4, CoinId = assetCoinId },
                            new { Id = 0, Value = 20, Multiplier = 5, CoinId = assetCoinId },
                            new { Id = 0, Value = 25, Multiplier = 6, CoinId = assetCoinId }
                        },
                        KicksCount = 5,
                        PenaltyPrizeGroups = new List<object>
                        {
                            new
                            {
                                Id = 0,
                                Name = "Penalty Prize Group of Promotion 1",
                                ConfigurationId = 0,
                                Prizes = new List<object>
                                {
                                    new { Id = 0, Name = "Prize 1", Value = 0, Probability = 50, CoinId = assetCoinId, PrizeGroupId = 0, WheelIndex = 1 },
                                    new { Id = 0, Name = "Prize 2", Value = 5, Probability = 15, CoinId = coinInId, PrizeGroupId = 0, WheelIndex = 2 },
                                    new { Id = 0, Name = "Prize 3", Value = 5, Probability = 15, CoinId = assetCoinId, PrizeGroupId = 0, WheelIndex = 3 },
                                    new { Id = 0, Name = "Prize 4", Value = 1, Probability = 10, CoinId = coinInId, PrizeGroupId = 0, WheelIndex = 4 },
                                    new { Id = 0, Name = "Prize 5", Value = 3, Probability = 5, CoinId = assetCoinId, PrizeGroupId = 0, WheelIndex = 5 },
                                    new { Id = 0, Name = "Prize 6", Value = 1, Probability = 5, CoinId = coinInId, PrizeGroupId = 0, WheelIndex = 6 }
                                },
                                Configuration = 1
                            }
                        }
                    }

                }
            };

        return Ok();

        var promotionData = new CreatePromotionDto
        {
            //Promotion = new CreatePromotionCommandDto
            //{
            //    Title = "Super Winter Promo",
            //    StartDate = DateTimeOffset.UtcNow,
            //    EndDate = DateTimeOffset.UtcNow.AddDays(1),
            //    Description = "A promotion to engage users during the winter season.",
            //    CorrelationId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            //    TemplateId = "promo-template-001",
            //    SegmentIds = new List<string> { "default" },
            //    Coins = coins,
            //    ServiceIds = new List<int> { 1 }
            //},
            //Leaderboards = new List<CreateLeaderboardRecord>
            //{
            //        new CreateLeaderboardRecord
            //        {
            //            PromotionId = 1,
            //            PromotionName = "Super Winter Promo",
            //            Title = "Winter Leaderboard",
            //            Description = "Track user scores during the winter promotion.",
            //            EventType = EventType.External,
            //            RepeatType = RepeatType.EveryNDays,
            //            RepeatValue = 1,
            //            AnnouncementDate = DateTimeOffset.UtcNow.AddDays(-1),
            //            StartDate = DateTimeOffset.UtcNow,
            //            EndDate = DateTimeOffset.UtcNow.AddDays(30),
            //            Status = LeaderboardRecordStatus.Announced,
            //            IsGenerated = true,
            //            CorrelationId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            //            LeaderboardPrizes = new List<CreateLeaderboardRecordPrizeCommandItem>
            //            {
            //                new CreateLeaderboardRecordPrizeCommandItem
            //                {
            //                    CoinId = assetCoin.Name,
            //                    StartRank = 1,
            //                    EndRank = 3,
            //                    Amount = 1000
            //                },
            //                new CreateLeaderboardRecordPrizeCommandItem
            //                {
            //                    CoinId = coinIn.Name,
            //                    StartRank = 2,
            //                    EndRank = 5,
            //                    Amount = 500
            //                }
            //            },
            //            AggregationConfigurations = new List<AggregationConfiguration>
            //            {
            //                new AggregationConfiguration(
            //                    name: "Score Aggregation",
            //                    description: "Aggregates scores based on user activity.",
            //                    eventProducer: "GameServer",
            //                    aggregationSubscriber: "Leaderboard",
            //                    filters: new List<Filter>
            //                    {
            //                        new Filter("GameScore", Operator.Equals, "0")
            //                    },
            //                    aggregationType: AggregationType.Sum,
            //                    evaluationType: EvaluationType.SingleRule,
            //                    pointEvaluationRules: new List<PointEvaluationRule>
            //                    {
            //                        new PointEvaluationRule(1, 10),
            //                        new PointEvaluationRule(5, 50)
            //                    },
            //                    selectionField: "GameScore",
            //                    expiration: DateTime.UtcNow.AddDays(7),
            //                    promotionId: Guid.NewGuid().ToString(),
            //                    key: "leaderboard-daily"
            //                )
            //            }
            //        }
            //},
            //GameConfiguration = new List<Contracts.Dtos.Promotion.GameConfigDto>
            //{
            //    new Contracts.Dtos.Promotion.GameConfigDto
            //    {
            //        GameName = "wheel",
            //        GameConfiguration = new
            //        {
            //             Id = 0,
            //             Name = "wheel configuration",
            //             Value = 1,
            //             IsActive = true,
            //             PromotionId = 1,
            //             CorrelationId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            //             FromTemplateId = "string",
            //             Prices = new List<object>
            //             {
            //                 new
            //                 {
            //                     Id = 0,
            //                     Value = 10,
            //                     Multiplier = 2,
            //                     CoinId = "string"
            //                 }
            //             },
            //             WheelPrizeGroups = new List<object>
            //             {
            //                 new
            //                 {
            //                     Id = 0,
            //                     Sequence = new List<int> { 1, 2, 3 },
            //                     NextPrizeIndex = 0,
            //                     ConfigurationId = 0,
            //                     Prizes = new List<object>
            //                     {
            //                         new
            //                         {
            //                             Id = 0,
            //                             Value = 50,
            //                             Probability = 50,
            //                             CoinId = assetCoin.Name,
            //                             PrizeGroupId = 0,
            //                             Name = "Prize A",
            //                             WheelIndex = 1
            //                         },
            //                         new
            //                         {
            //                             Id = 0,
            //                             Value = 30,
            //                             Probability = 30,
            //                             CoinId = coinIn.Name,
            //                             PrizeGroupId = 0,
            //                             Name = "Prize B",
            //                             WheelIndex = 2
            //                         },
            //                         new
            //                         {
            //                             Id = 0,
            //                             Value = 20,
            //                             Probability = 20,
            //                             CoinId = assetCoin.Name,
            //                             PrizeGroupId = 0,
            //                             Name = "Prize C",
            //                             WheelIndex = 3
            //                         }
            //                     },
            //                     Name = "Wheel Prize Group 1"
            //                 }
            //                     }
            //                 },
            //    },
            //    new Contracts.Dtos.Promotion.GameConfigDto
            //    {
            //        GameName = "penaltykicks",
            //        GameConfiguration = new 
            //        {
            //            Id = 0,
            //            Name = "For Promotion 1",
            //            Description = "Main configuration of promotion 1",
            //            Value = 1,
            //            IsActive = true,
            //            PromotionId = 1111,
            //            CorrelationId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            //            Prices = new List<object>
            //            {
            //                new { Id = 0, Value = 5, Multiplier = 2, CoinId = assetCoin.Name },
            //                new { Id = 0, Value = 10, Multiplier = 3, CoinId = assetCoin.Name },
            //                new { Id = 0, Value = 15, Multiplier = 4, CoinId = assetCoin.Name },
            //                new { Id = 0, Value = 20, Multiplier = 5, CoinId = assetCoin.Name },
            //                new { Id = 0, Value = 25, Multiplier = 6, CoinId = assetCoin.Name }
            //            },
            //            KicksCount = 5,
            //            PenaltyPrizeGroups = new List<object>
            //            {
            //                new
            //                {
            //                    Id = 0,
            //                    Name = "Penalty Prize Group of Promotion 1",
            //                    ConfigurationId = 0,
            //                    Prizes = new List<object>
            //                    {
            //                        new { Id = 0, Name = "Prize 1", Value = 0, Probability = 50, CoinId = assetCoin.Name, PrizeGroupId = 0, WheelIndex = 1 },
            //                        new { Id = 0, Name = "Prize 2", Value = 5, Probability = 15, CoinId = coinIn.Name, PrizeGroupId = 0, WheelIndex = 2 },
            //                        new { Id = 0, Name = "Prize 3", Value = 5, Probability = 15, CoinId = assetCoin.Name, PrizeGroupId = 0, WheelIndex = 3 },
            //                        new { Id = 0, Name = "Prize 4", Value = 1, Probability = 10, CoinId = coinIn.Name, PrizeGroupId = 0, WheelIndex = 4 },
            //                        new { Id = 0, Name = "Prize 5", Value = 3, Probability = 5, CoinId = assetCoin.Name, PrizeGroupId = 0, WheelIndex = 5 },
            //                        new { Id = 0, Name = "Prize 6", Value = 1, Probability = 5, CoinId = coinIn.Name, PrizeGroupId = 0, WheelIndex = 6 }
            //                    },
            //                    Configuration = 1
            //                }
            //            }
            //        }

            //    }
            //}

        };
    }
}
