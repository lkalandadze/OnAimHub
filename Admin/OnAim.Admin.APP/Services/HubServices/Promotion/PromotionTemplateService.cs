using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public class PromotionTemplateService : IPromotionTemplateService
{
    private readonly IPromotionTemplateRepository _promotionTemplateRepository;
    private readonly ICoinTemplateService _coinTemplateService;
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;
    private readonly IGameTemplateService _gameTemplateService;
    private readonly ICoinService _coinService;
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;

    public PromotionTemplateService(IPromotionTemplateRepository promotionTemplateRepository,
        ICoinTemplateService coinTemplateService,
        ILeaderboardTemplateService leaderboardTemplateService,
        IGameTemplateService gameTemplateService,
        ICoinService coinService,
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository
        )
    {
        _promotionTemplateRepository = promotionTemplateRepository;
        _coinTemplateService = coinTemplateService;
        _leaderboardTemplateService = leaderboardTemplateService;
        _gameTemplateService = gameTemplateService;
        _coinService = coinService;
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
    }

    public async Task<ApplicationResult> GetAllPromotionTemplates(BaseFilter filter)
    {
        var temps = await _promotionTemplateRepository.GetPromotionTemplates();
        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = temps
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<PromotionTemplate>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult> GetPromotionTemplateById(string id)
    {
        var coin = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Template Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> CreatePromotionTemplate(CreatePromotionTemplate template)
    {
        var temp = new PromotionTemplate
        {
            StartDate = template.StartDate,
            EndDate = template.EndDate,
            Title = template.Title,
            Description = template.Description,
            SegmentIds = template.SegmentIds,
        };

        var coins = new List<Domain.HubEntities.Coin.Coin>();
        foreach (var item in template.Coins)
        {
            var coinId = $"{template.PromotionId}_{item.Name}"; // remove id and leave only name
            Domain.HubEntities.Coin.Coin coin; 

            switch (item.CoinType)
            {
                case Domain.HubEntities.Enum.CoinType.In:
                    coin = new InCoin(coinId, item.Name, item.Description, item.ImageUrl, template.PromotionId);
                    break;

                case Domain.HubEntities.Enum.CoinType.Out:
                    coin = new OutCoin(coinId, item.Name, item.Description, item.ImageUrl, template.PromotionId);

                    if (coin is OutCoin outCoin)
                    {
                        var withdrawOptions = await _withdrawOptionRepository.Query()
                            .Where(wo => outCoin.WithdrawOptions.Select(x => x.Id).Any(woId => woId == wo.Id))
                            .ToListAsync(); 

                        var withdrawOptionGroups = await _withdrawOptionGroupRepository.Query()
                            .Where(wog => outCoin.WithdrawOptionGroups.Select(x => x.Id).Any(wogId => wogId == wog.Id))
                            .ToListAsync();

                        outCoin.AddWithdrawOptions(withdrawOptions);
                        outCoin.AddWithdrawOptionGroups(withdrawOptionGroups);
                    }
                    break;

                case Domain.HubEntities.Enum.CoinType.Asset:
                    coin = new AssetCoin(coinId, item.Name, item.Description, item.ImageUrl, template.PromotionId);
                    break;

                case Domain.HubEntities.Enum.CoinType.Internal:
                    coin = new InternalCoin(coinId, item.Name, item.Description, item.ImageUrl, template.PromotionId);
                    break;

                default:
                    throw new ArgumentException($"Invalid coin type: {item.CoinType}");
            }

            coins.Add(coin);
        }

        temp.Coins = coins;    

        var leaderboards = new List<LeaderboardRecord>();
        foreach(var leaderboard in template.Leaderboards)
        {
            var leadTemplate = new LeaderboardRecord
            {
                Title = leaderboard.Title,
                Description = leaderboard.Description,
                AnnouncementDate = leaderboard.AnnouncementDuration,
                StartDate = leaderboard.StartDuration,
                EndDate = leaderboard.EndDuration,
            };
            leaderboards.Add(leadTemplate);
            temp.Leaderboards = leaderboards;
        }

        var games = new List<GameConfigurationTemplate>();
        foreach (var item in template.Games)
        {
            //var conf = await _gameTemplateService.CreateGameConfigurationTemplate(item);
            //games.Add(conf);
            //temp.Games = games;
        }

        await _promotionTemplateRepository.AddPromotionTemplateAsync(temp);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeletePromotionTemplate(string id)
    {
        var template = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (template == null)
        {
            throw new NotFoundException("Template Not Found");
        }

        template.Delete();

        await _promotionTemplateRepository.UpdatePromotionTemplateAsync(id, template);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdatePromotionTemplate(UpdatePromotionTemplateDto update)
    {
        var template = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(update.Id);

        if (template == null || template.IsDeleted)
        {
            throw new NotFoundException($"template with the specified ID: [{update.Id}] was not found.");
        }
       
        await _promotionTemplateRepository.UpdatePromotionTemplateAsync(update.Id, template);

        return new ApplicationResult { Success = true };
    }
}