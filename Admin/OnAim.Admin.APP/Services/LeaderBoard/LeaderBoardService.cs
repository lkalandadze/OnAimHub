using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using MassTransit.Initializers;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderBoardService : ILeaderBoardService
{
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardTemplate> _leaderboardTemplateRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardRecord> _leaderboardRecordRepository;
    private readonly ILeaderBoardReadOnlyRepository<Prize> _prizeRepository;
    private readonly LeaderBoardApiClientOptions _options;
    private readonly ILeaderBoardApiClient _httpClientService;

    public LeaderBoardService(
        ILeaderBoardReadOnlyRepository<LeaderboardTemplate> leaderboardTemplateRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardRecord> leaderboardRecordRepository,
        ILeaderBoardReadOnlyRepository<Prize> prizeRepository,
        IOptions<LeaderBoardApiClientOptions> options,
        ILeaderBoardApiClient httpClientService
        )
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _prizeRepository = prizeRepository;
        _options = options.Value;
        _httpClientService = httpClientService;
    }

    public async Task<ApplicationResult> GetLeaderBoardTemplates(BaseFilter filter)
    {
        var sortableFields = new List<string> { "Id", "Name" };
        var leaderboardTemplates = _leaderboardTemplateRepository.Query();

        var totalCount = await leaderboardTemplates.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        bool sortDescending = filter?.SortDescending ?? false;

        if (filter?.SortBy == "Id" || filter?.SortBy == "id")
        {
            leaderboardTemplates = sortDescending
                ? leaderboardTemplates.OrderByDescending(x => x.Id)
                : leaderboardTemplates.OrderBy(x => x.Id);
        }
        else if (filter?.SortBy == "name" || filter?.SortBy == "Name")
        {
            leaderboardTemplates = sortDescending
                ? leaderboardTemplates.OrderByDescending(x => x.Name)
                : leaderboardTemplates.OrderBy(x => x.Name);
        }

        var res = leaderboardTemplates
            .Select(x => new LeaderBoardTemplateDto
            {
                Id = x.Id,
                Name = x.Name,
                AnnounceIn = x.AnnounceIn,
                Description = x.Description,
                StartTime = x.StartTime,
                StartIn = x.StartIn,
                EndIn = x.EndIn,
                Segments = 0,
                Usage = 0,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<LeaderBoardTemplateDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = sortableFields,
            },
        };
    }

    public async Task<ApplicationResult> GetLeaderboardTemplateById(int id)
    {
        var template = await _leaderboardTemplateRepository
            .Query(x => x.Id == id)
            .Include(x => x.LeaderboardTemplatePrizes)
            .ThenInclude(x => x.Prize)
            .FirstOrDefaultAsync();

        if (template == null)
            throw new NotFoundException("");


        var res = new TemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            StartTime = template.StartTime,
            StartIn = template.StartIn,
            EndIn = template.EndIn,
            AnnounceIn= template.AnnounceIn,
            Prizes = template.LeaderboardTemplatePrizes.Select(x => new TemplatePrizeDto
            {
                Id= x.Id,
                StartRank = x.StartRank,
                EndRank = x.EndRank,
                Prize = x.Prize.Name,
                Amount = x.Amount,
            }).ToList(),
        };

        return new ApplicationResult { Data = res, Success = true };
    }

    public async Task<ApplicationResult> GetAllLeaderBoard(LeaderBoardFilter? filter)
    {
        var leaderboards = _leaderboardRecordRepository.Query();

        if (filter?.LeaderBoardId.HasValue == true)
        {
            leaderboards = leaderboards.Where(x => x.Id == filter.LeaderBoardId);
        }
        if (!string.IsNullOrEmpty(filter?.Status))
        {
            leaderboards = leaderboards.Where(x => x.Status.ToString() == filter.Status);
        }
        if (filter?.StartDate.HasValue == true)
        {
            leaderboards = leaderboards.Where(x => x.StartDate >= filter.StartDate);
        }
        if (filter?.EndDate.HasValue == true)
        {
            leaderboards = leaderboards.Where(x => x.EndDate <= filter.EndDate);
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
               Name = x.Name,
               Status = x.Status.ToString(),
               EndsOn = x.EndDate,
               Prizes = x.LeaderboardRecordPrizes.Select(x => new PrizeDto
               {
                   PrizeType = x.Prize.Name,
                   Count = 0
               }).ToList(),
               LeaderBoardItems = x.LeaderboardProgresses
               .Skip((itemsPageNumber - 1) * itemsPageSize)
               .Take(itemsPageSize)
               .Select(xx => new LeaderBoardItemsDto
               {
                   PlayerId = xx.PlayerId,
                   UserName = xx.PlayerUsername,
                   Segment = null,
                   Place = 0,
                   Score = 0,
                   PrizeType = null,
                   PrizeValue = null,
               }).ToList(),
               PrizeConfigurations = x.LeaderboardRecordPrizes.Select(xxx => new PrizeConfigurationsDto
               {
                   Id = xxx.Id,
                   Name = xxx.Prize.Name,
                   PrizeId = xxx.PrizeId,
                   EndRank = xxx.EndRank,
                   StartRank = xxx.StartRank,
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
            .Include(x => x.LeaderboardRecordPrizes)
            .ThenInclude(x => x.Prize)
            .FirstOrDefaultAsync();

        if (leaderboard == null)
            throw new NotFoundException("");
        var res = new LeaderBoardData
        {
            Id = leaderboard.Id,
            Name = leaderboard.Name,
            Description = leaderboard.Description,
            CreationDate = leaderboard.CreationDate,
            AnnouncementDate = leaderboard.AnnouncementDate,
            StartDate = leaderboard.StartDate,
            EndDate = leaderboard.EndDate,
            LeaderboardType = leaderboard.LeaderboardType.ToString(),
            Prizes = leaderboard.LeaderboardRecordPrizes.Select(x => new TemplatePrizeDto
            {
                Id = x.Id,
                Amount = x.Amount,
                StartRank = x.StartRank,
                EndRank = x.EndRank,
                Prize = x.Prize.Name,
            }).ToList(),
        };

        return new ApplicationResult { Data = res, Success = true };
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

    public async Task<ApplicationResult> CreateTemplate(CreateLeaderboardTemplateDto createLeaderboardTemplateDto)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}CreateLeaderboardTemplate", createLeaderboardTemplateDto);

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> UpdateTemplate(UpdateLeaderboardTemplateDto updateLeaderboardTemplateDto)
    {
        var result = await _httpClientService.PutAsJson($"{_options.BaseApiAddress}{_options.Endpoint}UpdateLeaderboardTemplates", updateLeaderboardTemplateDto);

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordDto createLeaderboardRecordDto)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}CreateLeaderboardRecord", createLeaderboardRecordDto);

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordDto updateLeaderboardRecordDto)
    {
        var result = await _httpClientService.PutAsJson($"{_options.Endpoint}UpdateLeaderboardRecord", updateLeaderboardRecordDto);

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        var result = await _httpClientService.Get<object>($"{_options.Endpoint}GetCalendar?StartDate={startDate}&EndDate={endDate}");

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> GetLeaderboardSchedules(int? pageNumber, int? pageSize)
    {
        var result = await _httpClientService.Get<object>($"{_options.Endpoint}GetLeaderboardSchedules?PageNumber={pageNumber}&PageSize={pageSize}");

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> CreateLeaderboardSchedule(CreateLeaderboardScheduleDto createLeaderboardSchedule)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}CreateLeaderboardSchedule", createLeaderboardSchedule);

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleDto updateLeaderboardSchedule)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}UpdateLeaderboardSchedule", updateLeaderboardSchedule);

        return new ApplicationResult { Data = result, Success = true };
    }
}
public class TemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public List<TemplatePrizeDto> Prizes { get; set; }
}
public class TemplatePrizeDto
{
    public int Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string Prize { get; set; }
    public string PrizeType { get; set; }
    public int Amount { get; set; }
}
public class LeaderBoardData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string LeaderboardType { get; set; }
    public List<TemplatePrizeDto> Prizes { get; set; }
}

public sealed class CreateLeaderboardScheduleDto
{
    public RepeatType RepeatType { get; set; } 
    public int? RepeatValue { get; set; }
    public DateOnly? SpecificDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int LeaderboardTemplateId { get; set; }
}
public enum RepeatType
{
    SingleDate = 0,
    EveryNDays = 1,
    DayOfWeek = 2,
    DayOfMonth = 3
}

public sealed class UpdateLeaderboardScheduleDto
{
    public int Id { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
}
public enum LeaderboardScheduleStatus
{
    Active = 0,
    Completed = 1,
    Cancelled = 2,
}
