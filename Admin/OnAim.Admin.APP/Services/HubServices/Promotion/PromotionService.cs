using AggregationService.Application.Models.AggregationConfigurations;
using AggregationService.Application.Models.Filters;
using AggregationService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.FileServices;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Models;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;
using System.Text.Json;

namespace OnAim.Admin.APP.Services.Hub.Promotion;

public class PromotionService : BaseService, IPromotionService
{
    private readonly IPromotionRepository<Domain.HubEntities.Promotion> _promotionRepository;
    private readonly IPromotionRepository<Domain.HubEntities.Coin.Coin> _coinRepo;
    private readonly IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository;
    private readonly IReadOnlyRepository<Transaction> _transactionRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;
    private readonly IHubApiClient _hubApiClient;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IPromotionRepository<Service> _serviceRepository;
    private readonly ILogger<PromotionService> _logger;
    private readonly LeaderboardClientService _leaderboardClientService;
    private readonly IGameService _gameService;
    private readonly ILeaderBoardApiClient _leaderBoardApiClient;
    private readonly IAggregationClient _aggregationClient;
    private readonly AggregationClientOptions _aggregationClientOptions;
    private readonly LeaderBoardApiClientOptions _leaderBoardApiClientOptions;
    private readonly HubApiClientOptions _options;

    public PromotionService(
        IPromotionRepository<Domain.HubEntities.Promotion> promotionRepository,
        IPromotionRepository<Domain.HubEntities.Coin.Coin> coinRepo,
        IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> playerRepository,
        IReadOnlyRepository<Transaction> transactionRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository,
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        ISecurityContextAccessor securityContextAccessor,
        IPromotionRepository<Service> serviceRepository,
        ILogger<PromotionService> logger,
        LeaderboardClientService leaderboardClientService,
        IGameService gameService,
        IOptions<LeaderBoardApiClientOptions> leaderBoardApiClientOptions,
        ILeaderBoardApiClient leaderBoardApiClient,
        IAggregationClient aggregationClient,
        IOptions<AggregationClientOptions> aggregationClientOptions
        )
    {
        _promotionRepository = promotionRepository;
        _coinRepo = coinRepo;
        _playerRepository = playerRepository;
        _transactionRepository = transactionRepository;
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _hubApiClient = hubApiClient;
        _securityContextAccessor = securityContextAccessor;
        _serviceRepository = serviceRepository;
        _logger = logger;
        _leaderboardClientService = leaderboardClientService;
        _gameService = gameService;
        _leaderBoardApiClient = leaderBoardApiClient;
        _aggregationClient = aggregationClient;
        _aggregationClientOptions = aggregationClientOptions.Value;
        _leaderBoardApiClientOptions = leaderBoardApiClientOptions.Value;
        _options = options.Value;
    }

    public async Task<ApplicationResult<PaginatedResult<PromotionDto>>> GetAllPromotions(PromotionFilter filter)
    {
        var promotions = _promotionRepository.Query(
                         x =>
                                      string.IsNullOrEmpty(filter.Name) || EF.Functions.Like(x.Title, $"{filter.Name}%")
                         )
            .Include(x => x.Coins)
            .Include(x => x.Views)
            .AsNoTracking();

        if (filter.Status.HasValue)
            promotions = promotions.Where(x => x.Status == (Domain.HubEntities.Enum.PromotionStatus)filter.Status.Value);

        if (filter.StartDate.HasValue)
            promotions = promotions.Where(x => x.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            promotions = promotions.Where(x => x.EndDate <= filter.EndDate.Value);

        var totalCount = await promotions.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;
        bool sortDescending = filter.SortDescending.GetValueOrDefault();

        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Id)
                : promotions.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "name" || filter.SortBy == "Name")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Title)
                : promotions.OrderBy(x => x.Title);
        }
        else if (filter.SortBy == "status" || filter.SortBy == "Status")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Status)
                : promotions.OrderBy(x => x.Status);
        }
        else if (filter.SortBy == "startDate" || filter.SortBy == "StartDate")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.StartDate)
                : promotions.OrderBy(x => x.StartDate);
        }
        else if (filter.SortBy == "endDate" || filter.SortBy == "EndDate")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.EndDate)
                : promotions.OrderBy(x => x.EndDate);
        }

        var res = promotions
            .Select(x => new PromotionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = (PromotionStatus)x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Segments = x.Segments.Select(s => s.Id).ToList(),
                PromotionCoins = x.Coins.Select(xx => new PromotionCoinDto
                {
                    Id = xx.Id,
                    Name = xx.Name,
                    Description = xx.Description,
                    ImageUrl = xx.ImageUrl,
                    CoinType = (Contracts.Dtos.Coin.CoinType)xx.CoinType,
                }).ToList(),
                PageViews = x.Views.Select(x => x.Url).ToList(),
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);


        return new ApplicationResult<PaginatedResult<PromotionDto>>
        {
            Success = true,
            Data = new PaginatedResult<PromotionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = new List<string> { "Id", "Name", "Status" },
            },
        };
    }

    public async Task<object> GetAllPromotionGames(int promotionId, BaseFilter? filter)
    {
        object response = await _hubApiClient.Get<object>($"{_options.Endpoint}Admin/AllGames?Name=&PromotionId={promotionId}");

        return response;
    }

    public async Task<ApplicationResult<PaginatedResult<PlayerListDto>>> GetPromotionPlayers(int promotionId, PlayerFilter filter)
    {
        var sortableFields = new List<string> { "Id", "UserName" };

        var data = _promotionRepository
            .Query()
            .Where(x => x.Id == promotionId && !x.IsDeleted)
            .Include(x => x.Segments)
            .SelectMany(x => x.Segments)
            .SelectMany(x => x.Players)
            .Distinct();

        if (filter.IsBanned.HasValue)
            data = data.Where(x => x.IsBanned == filter.IsBanned.Value);

        if (filter.SegmentIds?.Any() == true)
            data = data.Where(x => x.Segments.Any(ur => filter.SegmentIds.Contains(ur.Id)));

        if (filter.DateFrom.HasValue)
            data = data.Where(x => x.LastVisitedOn >= filter.DateFrom.Value);

        if (filter.DateFrom.HasValue)
            data = data.Where(x => x.LastVisitedOn <= filter.DateTo.Value);

        var totalCount = await data.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;
        bool sortDescending = filter.SortDescending.GetValueOrDefault();

        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            data = sortDescending
                ? data.OrderByDescending(x => x.Id)
                : data.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "userName" || filter.SortBy == "UserName")
        {
            data = sortDescending
                ? data.OrderByDescending(x => x.UserName)
                : data.OrderBy(x => x.UserName);
        }

        var promotionPlayers = data.Select(x => new PlayerListDto
        {
            Id = x.Id,
            UserName = x.UserName,
            LastVisit = x.LastVisitedOn,
            Segment = x.Segments.OrderByDescending(ps => ps.PriorityLevel)
                            .Select(ps => ps.Id)
                            .FirstOrDefault(),
            RegistrationDate = x.RegistredOn,
            IsBanned = x.IsBanned,
        })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PlayerListDto>>
        {
            Success = true,
            Data = new PaginatedResult<PlayerListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await promotionPlayers.ToListAsync(),
                SortableFields = sortableFields,
            },
        };
    }

    public async Task<ApplicationResult<PaginatedResult<PlayerTransactionDto>>> GetPromotionPlayerTransaction(int promotionId, PlayerTransactionFilter filter)
    {
        var transaction = _transactionRepository.Query()
            .Include(x => x.Coin)
            .Where(x => x.PromotionId == promotionId);

        //if (filter.TransactionType != null)
        //    transaction = transaction.Where(x => x.Type == filter.TransactionType);

        //if (filter.TransactionStatus != null)
        //    transaction = transaction.Where(x => x.Status == filter.TransactionStatus);

        var totalCount = await transaction.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var item = transaction.Select(x => new PlayerTransactionDto
        {
            Id = x.Id,
            PlayerId = x.PlayerId,
            TransactionType = x.Type,
            Amount = x.Amount,
            Coin = x.Coin.Name,
            Status = x.Status,
        })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PlayerTransactionDto>>
        {
            Success = true,
            Data = new PaginatedResult<PlayerTransactionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await item.ToListAsync(),
                SortableFields = new List<string>(),
            },
        };
    }

    public async Task<ApplicationResult<PromotionLeaderboardDto<object>>> GetPromotionLeaderboards(int promotionId, BaseFilter filter)
    {
        var data = _leaderboardRecordRepository.Query().Include(x => x.LeaderboardSchedule).Where(x => x.PromotionId == promotionId);

        var allPrizes = await data.SelectMany(x => x.LeaderboardRecordPrizes)
            .GroupBy(prize => prize.CoinId)
            .Select(group => new PromotionLeaderboardPrizesDto
            {
                Prize = group.Key,
                Count = group.Count()
            })
            .ToListAsync();

        var totalCount = await data.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var items = data.Select(x => new PromotionLeaderboardItemsDto
        {
            Id = x.Id,
            Title = x.Title,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
        }).Skip((pageNumber - 1) * pageSize)
          .Take(pageSize);


        var leaderboardResult = new PromotionLeaderboardDto<object>
        {
            Leaderboards = new PaginatedResult<PromotionLeaderboardItemsDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await items.ToListAsync(),
                SortableFields = new List<string> { "Id", "Name", "Status" },
            },
            Prizes = allPrizes
        };
    

        return new ApplicationResult<PromotionLeaderboardDto<object>>
        {
            Success = true,
            Data = leaderboardResult
        };
    }

    public async Task<ApplicationResult<PaginatedResult<PromotionLeaderboardDetailDto>>> GetPromotionLeaderboardDetails(int leaderboardId, BaseFilter filter)
    {
        var data = _leaderboardResultRepository
            .Query()
            .Where(x => x.Id == leaderboardId)
            .Include(x => x.LeaderboardRecord)
            .ThenInclude(x => x.LeaderboardRecordPrizes);

        var totalCount = await data.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var items = data.Select(x => new PromotionLeaderboardDetailDto
        {
            Id = x.Id,
            PlayerId = x.PlayerId,
            UserName = x.PlayerUsername,
            Segment = null,
            Place = x.Placement,
            Score = x.Amount,
            PrizeType = x.LeaderboardRecord.LeaderboardRecordPrizes.Select(x => x.CoinId).FirstOrDefault(),
            PrizeValue = x.LeaderboardRecord.LeaderboardRecordPrizes.Select(x => x.Amount).FirstOrDefault(),
        })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PromotionLeaderboardDetailDto>>
        {
            Success = true,
            Data = new PaginatedResult<PromotionLeaderboardDetailDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await items.ToListAsync(),
                SortableFields = new List<string>(),
            },
        };
    }

    public async Task<ApplicationResult<PromotionDto>> GetPromotionById(int id)
    {
        var promotion = await _promotionRepository.Query().Include(x => x.Segments).FirstOrDefaultAsync(x => x.Id == id);

        if (promotion == null) throw new NotFoundException("promotion not found");

        var coins = _coinRepo.Query().Where(x => x.PromotionId == promotion.Id).ToList();

        var result = new PromotionDto
        {
            Id = promotion.Id,
            Title = promotion.Title,
            Description = promotion.Description,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate,
            PromotionCoins = coins.Select(xx => new PromotionCoinDto
            {
                Id = xx.Id,
                Name = xx.Name,
                Description = xx.Description,
                ImageUrl = xx.ImageUrl,               
                CoinType = (Contracts.Dtos.Coin.CoinType)xx.CoinType,
            }).ToList(),
            Segments = promotion.Segments.Select(s => s.Id).ToList(),
        };

        return new ApplicationResult<PromotionDto> { Success = true, Data = result };
    }

    public async Task<ApplicationResult<List<Service>>> GetAllService()
    {
        var service = _serviceRepository.Query();

        return new ApplicationResult<List<Service>>
        {
            Data = await service.ToListAsync(),
            Success = true
        };
    }

    public async Task<ApplicationResult<Guid>> CreatePromotion(CreatePromotionDto request)
    {
        var correlationId = Guid.NewGuid();
        var aggreg = new List<CreateAggregationConfigurationModel>();
        try
        {
            request.Promotion.CorrelationId = correlationId;
            int promotionId;
            int leaderboardId;
            try
            {
                var res = await CreatePromotionAsync(request.Promotion);
                promotionId = res.PromotionId;

                var mappedCoins = request.Promotion.Coins.Select(coin => CreateCoinModel.ConvertToEntity(coin, promotionId))
                                           .ToList();

                if (request.Promotion.Coins.FirstOrDefault(c => c.CoinType == Domain.HubEntities.Enum.CoinType.In) is CreateInCoinModel outCoinModel)
                {

                    var inCoin = mappedCoins.OfType<InCoin>()
                                             .FirstOrDefault(c => c.CoinType == Domain.HubEntities.Enum.CoinType.In);

                    foreach (var config in inCoin.AggregationConfiguration)
                    {
                        var aggregationModel = new CreateAggregationConfigurationModel
                        {
                            AggregationType = config.AggregationType,
                            SelectionField = config.SelectionField,
                            Filters = config.Filters.Select(x => new FilterModel
                            {
                                Property = x.Property,
                                Operator = x.Operator,
                                Value = x.Value
                            }).ToList(),
                            EvaluationType = config.EvaluationType,
                            PointEvaluationRules = config.PointEvaluationRules.Select(x => new AggregationService.Application.Models.PointEvaluationRules.PointEvaluationRuleModel
                            {
                                Point = x.Point,
                                Step = x.Step
                            }).ToList(),
                            EventProducer = config.EventProducer,
                            AggregationSubscriber = "Hub",
                            PromotionId = promotionId.ToString(),
                            Key = ""
                        };

                        aggreg.Add(aggregationModel);
                    }
                }

                _logger.LogInformation("Promotion created successfully: {PromotionId}", promotionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion.");
                await CompensateAsync(correlationId, null);

                return await Fail<Guid>(new Error
                {
                    Message = $"{ex.Message}",
                    Code = StatusCodes.Status500InternalServerError,
                });
            }

            if (request.Leaderboards.Count != 0)
            {
                try
                {
                    foreach (var leaderboard in request.Leaderboards)
                    {
                        leaderboard.CorrelationId = correlationId;
                        leaderboard.PromotionId = promotionId;

                        if (leaderboard != null)
                        {
                            try
                            {
                                var command = new CreateLeaderboardRecord
                                {
                                    AnnouncementDate = leaderboard.AnnouncementDate,
                                    CorrelationId = correlationId,
                                    Description = leaderboard.Description,
                                    EndDate = leaderboard.EndDate,
                                    EventType = leaderboard.EventType,
                                    IsGenerated = leaderboard.IsGenerated,
                                    PromotionName = leaderboard.PromotionName,
                                    LeaderboardPrizes = leaderboard.LeaderboardPrizes,
                                    PromotionId = promotionId,
                                    RepeatType = leaderboard.RepeatType,
                                    RepeatValue = leaderboard.RepeatValue,
                                    ScheduleId = leaderboard.ScheduleId,
                                    StartDate = leaderboard.StartDate,
                                    Status = leaderboard.Status,
                                    TemplateId = leaderboard.TemplateId,
                                    Title = leaderboard.Title,
                                    CreatedBy = _securityContextAccessor.UserId,
                                    AggregationConfigurations = null
                                };

                                var leaderboardResponse = await CreateLeaderboardRecordAsync(command);
                                leaderboardId = leaderboardResponse;

                                command.AggregationConfigurations = leaderboard.AggregationConfigurations;

                                foreach (var item in command.AggregationConfigurations)
                                {
                                    var aggregation = new CreateAggregationConfigurationModel
                                    {
                                        AggregationType = item.AggregationType,
                                        SelectionField = item.SelectionField,
                                        Filters = item.Filters.Select(x => new FilterModel
                                        {
                                            Property = x.Property,
                                            Operator = x.Operator,
                                            Value = x.Value
                                        }).ToList(),
                                        EvaluationType = item.EvaluationType,
                                        PointEvaluationRules = item.PointEvaluationRules.Select(x => new AggregationService.Application.Models.PointEvaluationRules.PointEvaluationRuleModel
                                        {
                                            Point = x.Point,
                                            Step = x.Step
                                        }).ToList(),
                                        EventProducer = item.EventProducer,
                                        Expiration = item.Expiration,
                                        PromotionId = promotionId.ToString(),
                                        Key = leaderboardId.ToString(),
                                        AggregationSubscriber = "Leaderboard"
                                    };

                                    aggreg.Add(aggregation);
                                }

                                try
                                {
                                    //await CreateAggregationConfiguration(aggreg);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error creating aggregation.");

                                    await CompensateAsync(correlationId, null);

                                    return await Fail<Guid>(new Error
                                    {
                                        Message = $"{ex.Message}",
                                        Code = StatusCodes.Status500InternalServerError,
                                    });
                                }

                                _logger.LogInformation("LeaderboardRecord created successfully: {LeaderboardRecord}", leaderboardResponse);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error creating leaderboard record.");
                                await CompensateAsync(correlationId, null);

                                return await Fail<Guid>(new Error
                                {
                                    Message = $"{ex.Message}",
                                    Code = StatusCodes.Status500InternalServerError,
                                });
                            }
                        }

                        _logger.LogInformation($"Leaderboard created successfully");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing leaderboards.");
                    await CompensateAsync(correlationId, null);

                    return await Fail<Guid>(new Error
                    {
                        Message = $"{ex.Message}",
                        Code = StatusCodes.Status500InternalServerError,
                    });
                }
            }

            if (request.GameConfiguration.Count != 0)
            {
                try
                {
                    foreach (var config in request.GameConfiguration)
                    {
                        JsonElement gameConfigElement = (JsonElement)config.GameConfiguration;

                        string jsonString = gameConfigElement.GetRawText();

                        JObject gameConfig = JObject.Parse(jsonString);
                        gameConfig["correlationId"] = correlationId;
                        gameConfig["promotionId"] = promotionId;

                        if (config != null)
                        {
                            try
                            {
                                var gameResponse = await CreateGameConfiguration(config.GameName, gameConfig);
                                _logger.LogInformation("Game configuration created successfully: {configs}", gameResponse);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error creating game configuration.");
                                await CompensateAsync(correlationId, config.GameName);

                                return await Fail<Guid>(new Error
                                {
                                    Message = $"{ex.Message}",
                                    Code = StatusCodes.Status500InternalServerError,
                                });
                            }
                        }

                        _logger.LogInformation("Game configuration created successfully: {Configuration}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing game configurations.");
                    await CompensateAsync(correlationId, null);

                    return await Fail<Guid>(new Error
                    {
                        Message = $"{ex.Message}",
                        Code = StatusCodes.Status500InternalServerError,
                    });
                }
            }

            _logger.LogInformation("Saga completed successfully.");
            return new ApplicationResult<Guid> { Success = true, Data = correlationId };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Saga execution.");
            await CompensateAsync(correlationId, null);

            return await Fail<Guid>(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> CreatePromotionView(CreatePromotionView create)
    {
        try
        {
            var hhub = new HubClient();

            var data = await hhub.PostAsJsonAndSerializeResultTo<Dictionary<string, object>>("HubApi/Admin/CreatePromotionView", create);

            if (data != null && data.TryGetValue("data", out var innerData))
            {
                if (innerData is JObject jObject)
                {
                    var viewUrl = jObject["viewUrl"]?.ToString();
                    if (!string.IsNullOrEmpty(viewUrl))
                    {
                        return new ApplicationResult<object> { Success = true, Data = viewUrl };
                    }
                }
            }

            return new ApplicationResult<object> { Success = false, Data = "viewUrl not found in the response data" };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the promotion view: " + ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> UpdatePromotionStatus(UpdatePromotionStatusDto update)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/UpdatePromotionStatus", update);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> DeletePromotion(int id)
    {
        try
        {
            var body = new
            {
                Id = id,
            };
            await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/SoftDeletePromotion", body);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<PromotionResponse> CreatePromotionAsync(CreatePromotionCommandDto request)
    {
        try
        {
            var promotionId = await _hubApiClient.PostAsJsonAndSerializeResultTo<PromotionResponse>($"{_options.Endpoint}Admin/CreatePromotion", request);
            return promotionId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<int> CreateLeaderboardRecordAsync(CreateLeaderboardRecord leaderboard)
    {
        try
        {
            var leadId = await _leaderBoardApiClient.PostAsJsonAndSerializeResultTo<int>($"{_leaderBoardApiClientOptions.Endpoint}CreateLeaderboardRecord", leaderboard);
            return leadId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create leaderboard.");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<object>> CreateGameConfiguration(string name, object configurationJson)
    {
        try
        {
            await _gameService.CreateConfiguration(name, configurationJson);
            return new ApplicationResult<object> { Success = true };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> CreateAggregationConfiguration(List<AggregationConfiguration> configuration)
    {
        try
        {
            await _aggregationClient.PostAsJson($"{_aggregationClientOptions.Endpoint}CreateConfigurations", configuration);
            return new ApplicationResult<object> { Success = true };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task CompensateAsync(Guid request, string? gameName)
    {
        try
        {
            await _hubApiClient.Delete($"{_options.Endpoint}Admin/DeletePromotion?CorrelationId={request}");
            await _leaderBoardApiClient.Delete($"{_leaderBoardApiClientOptions.Endpoint}DeleteLeaderboardRecord?CorrelationId={request}");
            if (gameName != null)
            {
                _gameService.DeleteConfiguration(gameName, request);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception(ex.Message, ex);
        }
    }
}
public class PlayerTransactionDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Coin { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }
}
public class PlayerTransactionFilter : BaseFilter
{
    public string SearchString { get; set; }
    public TransactionType TransactionType { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
}
public class CreatePromotionCommandDto
{
    public string Title { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Description { get; set; }
    public Guid CorrelationId { get; set; }
    public string? TemplateId { get; set; }
    public IEnumerable<string> SegmentIds { get; set; }
    public IEnumerable<CreateCoinModel> Coins { get; set; }
    public IEnumerable<int> ServiceIds { get; set; }
}
public class CreatePromotionDto
{
    public CreatePromotionCommandDto Promotion { get; set; }
    public List<CreateLeaderboardRecord>? Leaderboards { get; set; }
    public List<GameConfigDto>? GameConfiguration { get; set; }
}