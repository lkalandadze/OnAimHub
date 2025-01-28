using OnAim.Admin.APP.Services.LeaderBoardServices;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderBoardService : ILeaderBoardService
{
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;
    private readonly ILeaderBoardReadOnlyRepository<Domain.LeaderBoradEntities.Prize> _prizeRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecordPrize> _leaderboardRecordPrize;
    private readonly ILeaderBoardApiClient _leaderBoardApiClient;
    private readonly LeaderBoardApiClientOptions _leaderBoardApiClientOptions;

    //private readonly LeaderboardClientService _leaderboardClientService;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;

    public LeaderBoardService(
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository,
        ILeaderBoardReadOnlyRepository<Domain.LeaderBoradEntities.Prize> prizeRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecordPrize> leaderboardRecordPrize,
        IOptions<LeaderBoardApiClientOptions> leaderBoardApiClientOptions,
        ILeaderBoardApiClient leaderBoardApiClient,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository
        )
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _prizeRepository = prizeRepository;
        _leaderboardRecordPrize = leaderboardRecordPrize;
        _leaderBoardApiClient = leaderBoardApiClient;
        _leaderBoardApiClientOptions = leaderBoardApiClientOptions.Value;
        //_leaderboardClientService = leaderboardClientService;
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task<ApplicationResult<PaginatedResult<LeaderBoardListDto>>> GetAllLeaderBoard(LeaderBoardFilter? filter)
    {
        var leaderboards = _leaderboardRecordRepository.Query().Include(x => x.LeaderboardRecordPrizes);

        var prizes = _leaderboardRecordPrize.Query();

        if (filter?.LeaderBoardId.HasValue == true)
        {
            leaderboards = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<LeaderboardRecord, ICollection<LeaderboardRecordPrize>>)leaderboards.Where(x => x.Id == filter.LeaderBoardId);
        }
        if (!string.IsNullOrEmpty(filter?.Status))
        {
            leaderboards = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<LeaderboardRecord, ICollection<LeaderboardRecordPrize>>)leaderboards.Where(x => x.Status.ToString() == filter.Status);
        }
        if (filter?.StartDate.HasValue == true)
        {
            leaderboards = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<LeaderboardRecord, ICollection<LeaderboardRecordPrize>>)leaderboards.Where(x => x.StartDate >= filter.StartDate);
        }
        if (filter?.EndDate.HasValue == true)
        {
            leaderboards = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<LeaderboardRecord, ICollection<LeaderboardRecordPrize>>)leaderboards.Where(x => x.EndDate <= filter.EndDate);
        }

        var totalCount = await leaderboards.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = leaderboards
           .Select(x => new LeaderBoardListDto
           {
               Id = x.Id,
               Title = x.Title,
               Status = (Contracts.Dtos.LeaderBoard.LeaderboardRecordStatus)x.Status,
               Description = x.Description,
               EventType = (Contracts.Dtos.LeaderBoard.EventType)x.EventType,
               CreationDate = x.CreationDate,
               AnnouncementDate = x.AnnouncementDate,
               StartDate = x.StartDate,
               EndDate = x.EndDate,
               IsGenerated = x.IsGenerated,
               Prizes = x.LeaderboardRecordPrizes.Select(xx => new Contracts.Dtos.Game.PrizeDto
               {
                   Id = xx.Id,
                   Name = xx.CoinId,
                   Value = xx.Amount,
               }).ToList(),
           })
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult<PaginatedResult<LeaderBoardListDto>>
        {
            Success = true,
            Data = new PaginatedResult<LeaderBoardListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
            },
        };
    }

    public async Task<ApplicationResult<LeaderBoardData>> GetLeaderboardRecordById(int id)
    {
        var leaderboard = await _leaderboardRecordRepository.Query(x => x.Id == id)
            .FirstOrDefaultAsync();

        var prize = await _leaderboardRecordPrize.Query().Where(x => x.LeaderboardRecordId == leaderboard.Id).ToListAsync();

        var data = _leaderboardResultRepository
           .Query()
           .Where(x => x.Id == leaderboard.Id)
           .Include(x => x.LeaderboardRecord)
           .ThenInclude(x => x.LeaderboardRecordPrizes)
           .ToList();

        if (leaderboard == null)
            throw new NotFoundException("");

        var res = new LeaderBoardData
        {
            Id = leaderboard.Id,
            Title = leaderboard.Title,
            Description = leaderboard.Description,
            CreationDate = leaderboard.CreationDate,
            AnnouncementDate = leaderboard.AnnouncementDate,
            StartDate = leaderboard.StartDate,
            EndDate = leaderboard.EndDate,
            Prizes = prize.Select(x => new TemplatePrizeDto
            {
                Id = x.Id,
                Amount = x.Amount,
                StartRank = x.StartRank,
                CoinId = x.CoinId,
                EndRank = x.EndRank,
            }).ToList(),
            Players = data.Select(x => new PromotionLeaderboardDetailDto
            {
                PlayerId = x.PlayerId,
                UserName = x.PlayerUsername,
                Segment = null,
                Place = x.Placement,
                Score = x.Amount,
                PrizeType = x.LeaderboardRecord.LeaderboardRecordPrizes.Select(x => x.CoinId).FirstOrDefault(),
                PrizeValue = x.LeaderboardRecord.LeaderboardRecordPrizes.Select(x => x.Amount).FirstOrDefault(),
            }).ToList(),
        };

        return new ApplicationResult<LeaderBoardData> { Data = res, Success = true };
    }

    public async Task<ApplicationResult<bool>> CreateLeaderBoardRecord(CreateLeaderboardRecordCommand createLeaderboardRecordDto)
    {
        try
        {
            await _leaderBoardApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}CreateLeaderboardRecord", createLeaderboardRecordDto);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> UpdateLeaderBoardRecord(UpdateLeaderboardRecordCommand updateLeaderboardRecordDto)
    {
        try
        {
            await _leaderBoardApiClient.PutAsJson($"{_leaderBoardApiClientOptions.Endpoint}UpdateLeaderboardRecord", updateLeaderboardRecordDto);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> DeleteLeaderBoardRecord(DeleteLeaderboardRecordCommand delete)
    {
        try
        {
            await _leaderBoardApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}DeleteLeaderboardRecord", delete);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<object>> GetLeaderboardSchedules(int? pageNumber, int? pageSize)
    {
        try
        {
            var res = await _leaderBoardApiClient.Get<object>(
                $"{_leaderBoardApiClientOptions.Endpoint}GetLeaderboardSchedules?PageNumber={pageNumber}&PageSize={pageSize}");
            return new ApplicationResult<object> { Data = res, Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> CreateLeaderboardSchedule(CreateLeaderboardScheduleCommand createLeaderboardSchedule)
    {
        try
        {
            await _leaderBoardApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}CreateLeaderboardSchedule", createLeaderboardSchedule);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<bool>> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleCommand updateLeaderboardSchedule)
    {
        try
        {
            await _leaderBoardApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}UpdateLeaderboardSchedule", updateLeaderboardSchedule);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<object>> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        try
        {
            var res = await _leaderBoardApiClient.Get<object>(
                $"{_leaderBoardApiClientOptions.Endpoint}GetCalendar?StartDate={startDate}&EndDate={endDate}");

            return new ApplicationResult<object> { Data = res, Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<object>> GetAllPrizes()
    {
        var prizes = _prizeRepository.Query();

        return new ApplicationResult<object>
        {
            Success = true,
            Data = await prizes.ToListAsync(),
        };
    }
}
public class LeaderBoardData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<TemplatePrizeDto> Prizes { get; set; }
    public List<PromotionLeaderboardDetailDto> Players { get; set; }
}