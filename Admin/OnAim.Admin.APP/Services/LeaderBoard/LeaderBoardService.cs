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

    public async Task<ApplicationResult> GetLeaderBoardTemplates(BaseFilter? filter)
    {
        var leaderboardTemplates = _leaderboardTemplateRepository.Query();

        var totalCount = await leaderboardTemplates.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

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
            },
        };
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

    public async Task<ApplicationResult> Schedule(int templateId)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}schedule/{templateId}", "");

        return new ApplicationResult { Data = result, Success = true };
    }

    public async Task<ApplicationResult> Execute(int templateId)
    {
        var result = await _httpClientService.PostAsJson($"{_options.Endpoint}execute/{templateId}", "");

        return new ApplicationResult { Data = result, Success = true };
    }
}
