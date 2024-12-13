using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Infrasturcture;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.Hub.Promotion;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository<Domain.HubEntities.Promotion> _promotionRepository;
    private readonly IPromotionRepository<Domain.HubEntities.Coin.Coin> _coinRepo;
    private readonly HubClientService _hubClientService;
    private readonly SagaClient _sagaClient;
    private readonly IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository;
    private readonly IReadOnlyRepository<Transaction> _transactionRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;

    public PromotionService(
        IPromotionRepository<Domain.HubEntities.Promotion> promotionRepository,
        IPromotionRepository<Domain.HubEntities.Coin.Coin> coinRepo,
        HubClientService hubClientService,
        SagaClient sagaClient,
        IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository,
        IReadOnlyRepository<Transaction> transactionRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository
        )
    {
        _promotionRepository = promotionRepository;
        _coinRepo = coinRepo;
        _hubClientService = hubClientService;
        _sagaClient = sagaClient;
        this._playerRepository = _playerRepository;
        _transactionRepository = transactionRepository;
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task<ApplicationResult> GetAllPromotions(PromotionFilter filter)
    {
        var promotions = _promotionRepository.Query(x =>
                         string.IsNullOrEmpty(filter.Name) || EF.Functions.Like(x.Title, $"{filter.Name}%"))
            .Include(x => x.Coins)
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
                }).ToList()
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

    public async Task<ApplicationResult> GetAllPromotionGames(int promotionId, BaseFilter filter)
    {
        var data = new List<PromotionGameDto>
        {
            new PromotionGameDto
            {
                Id = 1,
                GameName = "Treasure Hunt",
                Description = "Find hidden treasures and earn rewards.",
                BetPrice = 100,
                Coins = "Gold, Silver"
            },
            new PromotionGameDto
            {
                Id = 2,
                GameName = "Spin & Win",
                Description = "Spin the wheel for a chance to win big.",
                BetPrice = 50,
                Coins = "Gold, Diamond"
            },
            new PromotionGameDto
            {
                Id = 3,
                GameName = "Battle Arena",
                Description = "Compete against other players to claim victory.",
                BetPrice = 150,
                Coins = "Platinum, Ruby"
            }
        };


        return new ApplicationResult { Data = data };
    }

    public async Task<ApplicationResult> GetPromotionPlayers(int promotionId, PlayerFilter filter)
    {
        var sortableFields = new List<string> { "Id", "UserName" };

        var data = _promotionRepository
            .Query()
            .Where(x => x.Id == promotionId && !x.IsDeleted)
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

    public async Task<ApplicationResult> GetPromotionPlayerTransaction(int playerId, PlayerTransactionFilter filter)
    {
        //var data = await _playerRepository.Query().FirstOrDefaultAsync(x => x.Id == playerId);
        var transaction = _transactionRepository
            .Query()
            .Where(x => x.PlayerId == playerId);

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
        var data = await _leaderboardRecordRepository
            .Query()
            .Include(x => x.LeaderboardProgresses)
            .FirstOrDefaultAsync(x => x.Id == leaderboardId);

        var item = new PromotionLeaderboardDetailDto
        {
            //PlayerId = data.LeaderboardProgresses.Select(x => x.PlayerId).FirstOrDefault(),
            //Segment = null,
            //Place = 0,
            //Score = 0,
            //PrizeType = data.LeaderboardRecordPrizes.Select(x => new ),
            //PrizeValue = null,
        };

        return new ApplicationResult { Data = data };
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
            await _sagaClient.SagaAsync(create);
            return new ApplicationResult { Success = true };
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
            await _hubClientService.CreatePromotionViewAsync(create);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdatePromotionStatus(UpdatePromotionStatusCommand update)
    {
        try
        {
            await _hubClientService.UpdatePromotionStatusAsync(update);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> DeletePromotion(SoftDeletePromotionCommand command)
    {
        try
        {
            await _hubClientService.SoftDeletePromotionAsync(command);
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
    public decimal Amount { get; set; }
}
public record PlayerTransactionFilter : BaseFilter
{
    public string SearchString { get; set; }
    public TransactionType TransactionType { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
}
public class PromotionLeaderboardDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Place { get; set; }
    public RepeatType RepeatType { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
public class PromotionLeaderboardDetailDto
{
    //public int Id { get; set; }
    //public int PlayerId { get; set; }
    //public string UserName { get; set; }
    //public string Segment {  get; set; }
    //public int Place {  set; get; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; }
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; }
    //public decimal Score { get; set; }
    //public PrizeType PrizeType { get; set; }
    //public string PrizeValue { get; set; }
}
public class PromotionGameDto
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public string Description { get; set; }
    public int BetPrice { get; set; }
    public string Coins { get; set; }
}