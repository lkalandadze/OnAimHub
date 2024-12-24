using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OnAim.Admin.APP.Services.FileServices;
using OnAim.Admin.APP.Services.Game;
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
using OnAim.Admin.Domain.HubEntities.Models;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.Hub.Promotion;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository<Domain.HubEntities.Promotion> _promotionRepository;
    private readonly IPromotionRepository<Domain.HubEntities.Coin.Coin> _coinRepo;
    private readonly IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository;
    private readonly IReadOnlyRepository<Transaction> _transactionRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;
    private readonly ISagaApiClient _sagaApiClient;
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;
    private readonly SagaApiClientOptions _sagaOptions;

    public PromotionService(
        IPromotionRepository<Domain.HubEntities.Promotion> promotionRepository,
        IPromotionRepository<Domain.HubEntities.Coin.Coin> coinRepo,
        IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> playerRepository,
        IReadOnlyRepository<Transaction> transactionRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository,
        ISagaApiClient sagaApiClient,
        IOptions<SagaApiClientOptions> sagaOptions,
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options
        )
    {
        _promotionRepository = promotionRepository;
        _coinRepo = coinRepo;
        _playerRepository = playerRepository;
        _transactionRepository = transactionRepository;
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _sagaApiClient = sagaApiClient;
        _hubApiClient = hubApiClient;
        _options = options.Value;
        _sagaOptions = sagaOptions.Value;
    }

    public async Task<ApplicationResult> GetAllPromotions(PromotionFilter filter)
    {
        var promotions = _promotionRepository.Query(x =>
                         string.IsNullOrEmpty(filter.Name) || EF.Functions.Like(x.Title, $"{filter.Name}%"))
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
                Status = (Contracts.Dtos.Promotion.PromotionStatus)x.Status,
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


        return new ApplicationResult
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

    public async Task<ApplicationResult> GetAllPromotionGames(int promotionId, BaseFilter? filter)
    {
        var response = await _hubApiClient.Get<string>($"{_options.Endpoint}Admin/AllGames?Name=&PromotionId={promotionId}");

        return new ApplicationResult { Data = response };
    }

    public async Task<ApplicationResult> GetPromotionPlayers(int promotionId, PlayerFilter filter)
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

        return new ApplicationResult
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

    public async Task<ApplicationResult> GetPromotionPlayerTransaction(int promotionId, PlayerTransactionFilter filter)
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

        return new ApplicationResult
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

    public async Task<ApplicationResult> GetPromotionLeaderboards(int promotionId, BaseFilter filter)
    {
        var data = _leaderboardRecordRepository.Query().Where(x => x.PromotionId == promotionId);

        var totalCount = await data.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var items = data.Select(x => new PromotionLeaderboardDto
        {
            Id = x.Id,
            Title = x.Title,
            Place = 0,
            //RepeatType = (RepeatType)x.LeaderboardSchedule.RepeatType,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
        }).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<PromotionLeaderboardDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await items.ToListAsync(),
                SortableFields = new List<string>(),
            },
        };
    }

    public async Task<ApplicationResult> GetPromotionLeaderboardDetails(int leaderboardId, BaseFilter filter)
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

        return new ApplicationResult
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

    public async Task<ApplicationResult> GetPromotionById(int id)
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


        return new ApplicationResult { Success = true, Data = result };
    }

    public async Task<ApplicationResult> CreatePromotion(CreatePromotionDto create)
    {
        try
        {
            var res = await _sagaApiClient.PostAsJsonAndSerializeResultTo<object>($"{_sagaOptions.Endpoint}", create);
            return new ApplicationResult { Success = true, Data = res };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> CreatePromotionView(CreatePromotionView create)
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
                        return new ApplicationResult { Success = true, Data = viewUrl };
                    }
                }
            }

            return new ApplicationResult { Success = false, Data = "viewUrl not found in the response data" };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the promotion view: " + ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdatePromotionStatus(UpdatePromotionStatusDto update)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/UpdatePromotionStatus", update);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> DeletePromotion(int id)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/SoftDeletePromotion", id);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
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
public class CreatePromotionDto
{
    public CreatePromotionCommandDto Promotion { get; set; }
    public List<CreateLeaderboardRecord>? Leaderboards { get; set; }
    public List<GameConfigDto>? GameConfiguration { get; set; }
}
public class GameConfigDto
{
    public string GameName { get; set; }
    public GameConfigurationDto GameConfiguration { get; set; }
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
}