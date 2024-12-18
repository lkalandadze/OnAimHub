using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using MassTransit.Initializers;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.APP.Services.Hub.Promotion;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderBoardService : ILeaderBoardService
{
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;
    private readonly ILeaderBoardReadOnlyRepository<Prize> _prizeRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecordPrize> _leaderboardRecordPrize;
    private readonly LeaderboardClientService _leaderboardClientService;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;

    public LeaderBoardService(
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository,
        ILeaderBoardReadOnlyRepository<Prize> prizeRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecordPrize> leaderboardRecordPrize,
        LeaderboardClientService leaderboardClientService,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository
        )
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _prizeRepository = prizeRepository;
        _leaderboardRecordPrize = leaderboardRecordPrize;
        _leaderboardClientService = leaderboardClientService;
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task<ApplicationResult> GetAllLeaderBoard(LeaderBoardFilter? filter)
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

        var itemsPageNumber = filter?.ItemsPageNumber ?? 1;
        var itemsPageSize = filter?.ItemsPageSize ?? 10;

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
                   Amount = xx.Amount,
               }).ToList(),
           })
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
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

    public async Task<ApplicationResult> GetLeaderboardRecordById(int id)
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

        return new ApplicationResult { Data = res, Success = true };
    }

    public async Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordCommand createLeaderboardRecordDto)
    {
        try
        {
            await _leaderboardClientService.CreateLeaderboardRecordAsync(createLeaderboardRecordDto);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordCommand updateLeaderboardRecordDto)
    {
        try
        {
            await _leaderboardClientService.UpdateLeaderboardRecordAsync(updateLeaderboardRecordDto);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> DeleteLeaderBoardRecord(DeleteLeaderboardRecordCommand delete)
    {
        try
        {
            await _leaderboardClientService.DeleteLeaderboardRecordAsync(delete);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> GetLeaderboardSchedules(int? pageNumber, int? pageSize)
    {
        try
        {
            await _leaderboardClientService.GetLeaderboardSchedulesAsync(pageNumber, pageSize);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> CreateLeaderboardSchedule(CreateLeaderboardScheduleCommand createLeaderboardSchedule)
    {
        try
        {
            await _leaderboardClientService.CreateLeaderboardScheduleAsync(createLeaderboardSchedule);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleCommand updateLeaderboardSchedule)
    {
        try
        {
            await _leaderboardClientService.UpdateLeaderboardScheduleAsync(updateLeaderboardSchedule);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        try
        {
            await _leaderboardClientService.GetCalendarAsync(startDate, endDate);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> GetAllPrizes()
    {
        var prizes = _prizeRepository.Query();

        return new ApplicationResult
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