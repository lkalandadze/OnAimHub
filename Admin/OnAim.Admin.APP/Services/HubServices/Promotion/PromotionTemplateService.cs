﻿using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Dtos.Withdraw;

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

    public async Task<ApplicationResult<PaginatedResult<PromotionTemplateListDto>>> GetAllPromotionTemplates(BaseFilter filter)
    {
        var temps = await _promotionTemplateRepository.GetPromotionTemplates();
        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    temps = temps.Where(u => u.IsDeleted == false).ToList();
                    break;
                case HistoryStatus.Deleted:
                    temps = temps.Where(u => u.IsDeleted == true).ToList();
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var data = temps.Select(x => new PromotionTemplateListDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            IsDeleted = x.IsDeleted,
            DateDeleted = x.DateDeleted,
            Segments = (List<string>)x.SegmentIds,
            Coins = x.Coins.Select(xx =>
            {
                if (xx is OutCoin outCoin)
                {
                    return new CoinsPromTempDto
                    {
                        Name = outCoin.Name,
                        Description = outCoin.Description,
                        ImgUrl = outCoin.ImageUrl,
                        CoinType = (Contracts.Dtos.Coin.CoinType)outCoin.CoinType,
                        IsDeleted = (bool)outCoin.IsDeleted,
                        WithdrawOptions = outCoin.WithdrawOptions?.Select(o => new WithdrawOptionDto
                        {
                            Id = o.Id,
                            Title = o.Title,
                            Description = o.Description,
                            ImageUrl = o.ImageUrl,
                            //Endpoint = o.Endpoint,
                            //ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)o.ContentType,
                            ////EndpointContent = o.EndpointContent,
                            //WithdrawOptionGroups = o.WithdrawOptionGroups.Select(c => new WithdrawOptionGroupDto
                            //{
                            //    Title = c.Title,
                            //    Description = c.Description,
                            //    ImageUrl = c.ImageUrl,
                            //    PriorityIndex = c.PriorityIndex,
                            //}).ToList(),
                            //OutCoins = o.OutCoins?.Select(v => new OutCoinDto
                            //{
                            //    Name = v.Name,
                            //    Description = v.Description,
                            //    ImageUrl = v.ImageUrl,
                            //}).ToList() ?? new List<OutCoinDto>()
                        }).ToList() ?? new List<WithdrawOptionDto>(),
                        WithdrawOptiongroups = outCoin.WithdrawOptionGroups.Select(g => new WithdrawOptionGroupDto
                        {
                            Id = g.Id,
                            Title = g.Title,
                            Description = g.Description,
                            ImageUrl = g.ImageUrl,
                            PriorityIndex = g.PriorityIndex,
                            //OutCoins = g.OutCoins.Select(n => new OutCoinDto
                            //{
                            //    Name = n.Name,
                            //    Description = n.Description,
                            //    ImageUrl = n.ImageUrl
                            //}).ToList() ?? new List<OutCoinDto>(),
                            //WithdrawOptionIds = g.WithdrawOptions?.Select(x => x.Id).ToList() ?? new List<int>()
                        }).ToList() ?? new List<WithdrawOptionGroupDto>()
                    };
                }
                return new CoinsPromTempDto
                {
                    Name = xx.Name,
                    Description = xx.Description,
                    ImgUrl = xx.ImageUrl,
                    CoinType = (Contracts.Dtos.Coin.CoinType)xx.CoinType,
                    IsDeleted = xx.IsDeleted,
                    WithdrawOptions = null,
                    WithdrawOptiongroups = null
                };
            }).ToList(),
            Leaderboards = x.Leaderboards.Select(y => new LeaderboardsPromTempdto
            {
                Title = y.Title,
                Description = y.Description,
                EventType = (Contracts.Dtos.LeaderBoard.EventType)y.EventType,
                CreationDate = y.CreationDate,
                AnnouncementDate = y.AnnouncementDate.ToString(),
                StartDate = y.StartDate.ToString(),
                EndDate = y.EndDate.ToString(),
                Status = (Contracts.Dtos.LeaderBoard.LeaderboardRecordStatus)y.Status,
                IsGenerated = y.IsGenerated,
                ScheduleId = y.ScheduleId,
                LeaderboardTemplatePrizes = y.LeaderboardRecordPrizes.Select(z => new leaderboardTemplatePrizesDto
                {
                    Id = z.Id.ToString(),
                    Amount = z.Amount,
                    CoinId = z.CoinId,
                    StartRank = z.StartRank,
                    EndRank = z.EndRank
                }).ToList()

            }).ToList(),
            Games = x.Games.Select(w => new GameConfigurationPromTemplateListDto
            {
                GameName = w.Game,
                Configuration = w.Configuration,
            }).ToList(),
        });


        var res = data
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PromotionTemplateListDto>>
        {
            Success = true,
            Data = new PaginatedResult<PromotionTemplateListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult<PromotionTemplateListDto>> GetPromotionTemplateById(string id)
    {
        var template = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (template == null) throw new NotFoundException("Template Not Found");

        var data = new PromotionTemplateListDto
        {
            Id = template.Id,
            Title = template.Title,
            Description = template.Description,
            StartDate = template.StartDate,
            EndDate = template.EndDate,
            IsDeleted = template.IsDeleted,
            DateDeleted = template.DateDeleted,
            Segments = (List<string>)template.SegmentIds,
            Coins = template.Coins.Select(xx =>
            {
                if (xx is OutCoin outCoin)
                {
                    return new CoinsPromTempDto
                    {
                        Name = outCoin.Name,
                        Description = outCoin.Description,
                        ImgUrl = outCoin.ImageUrl,
                        CoinType = (Contracts.Dtos.Coin.CoinType)outCoin.CoinType,
                        IsDeleted = (bool)outCoin.IsDeleted,
                        WithdrawOptions = outCoin.WithdrawOptions?.Select(o => new WithdrawOptionDto
                        {
                            Id = o.Id,
                            Title = o.Title,
                            Description = o.Description,
                            ImageUrl = o.ImageUrl,
                            //Endpoint = o.Endpoint,
                            //ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)o.ContentType,
                            ////EndpointContent = o.EndpointContent,
                            //WithdrawOptionGroups = o.WithdrawOptionGroups.Select(c => new WithdrawOptionGroupDto
                            //{
                            //    Title = c.Title,
                            //    Description = c.Description,
                            //    ImageUrl = c.ImageUrl,
                            //    PriorityIndex = c.PriorityIndex,
                            //}).ToList(),
                            //OutCoins = o.OutCoins?.Select(v => new OutCoinDto
                            //{
                            //    Name = v.Name,
                            //    Description = v.Description,
                            //    ImageUrl = v.ImageUrl,
                            //}).ToList() ?? new List<OutCoinDto>()
                        }).ToList() ?? new List<WithdrawOptionDto>(),
                        WithdrawOptiongroups = outCoin.WithdrawOptionGroups.Select(g => new WithdrawOptionGroupDto
                        {
                            Id = g.Id,
                            Title = g.Title,
                            Description = g.Description,
                            ImageUrl = g.ImageUrl,
                            PriorityIndex = g.PriorityIndex,
                            //OutCoins = g.OutCoins.Select(n => new OutCoinDto
                            //{
                            //    Name = n.Name,
                            //    Description = n.Description,
                            //    ImageUrl = n.ImageUrl
                            //}).ToList() ?? new List<OutCoinDto>(),
                            //WithdrawOptionIds = g.WithdrawOptions?.Select(x => x.Id).ToList() ?? new List<int>()
                        }).ToList() ?? new List<WithdrawOptionGroupDto>()
                    };
                }
                return new CoinsPromTempDto
                {
                    Name = xx.Name,
                    Description = xx.Description,
                    ImgUrl = xx.ImageUrl,
                    CoinType = (Contracts.Dtos.Coin.CoinType)xx.CoinType,
                    IsDeleted = xx.IsDeleted,
                    WithdrawOptions = null,
                    WithdrawOptiongroups = null
                };
            }).ToList(),
            Leaderboards = template.Leaderboards.Select(y => new LeaderboardsPromTempdto
            {
                Title = y.Title,
                Description = y.Description,
                EventType = (Contracts.Dtos.LeaderBoard.EventType)y.EventType,
                CreationDate = y.CreationDate,
                AnnouncementDate = y.AnnouncementDate.ToString(),
                StartDate = y.StartDate.ToString(),
                EndDate = y.EndDate.ToString(),
                Status = (Contracts.Dtos.LeaderBoard.LeaderboardRecordStatus)y.Status,
                IsGenerated = y.IsGenerated,
                ScheduleId = y.ScheduleId,
                LeaderboardTemplatePrizes = y.LeaderboardRecordPrizes.Select(z => new leaderboardTemplatePrizesDto
                {
                    Amount = z.Amount,
                    CoinId = z.CoinId,
                    StartRank = z.StartRank,
                    EndRank = z.EndRank
                }).ToList()

            }).ToList(),
            Games = template.Games.Select(w => new GameConfigurationPromTemplateListDto
            {
                GameName = w.Game,
                Configuration = w.Configuration,
            }).ToList(),
        };

        return new ApplicationResult<PromotionTemplateListDto> { Success = true, Data = data };
    }

    public async Task<ApplicationResult<bool>> CreatePromotionTemplate(CreatePromotionTemplate template)
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

        var mappedCoins = template.Coins.Select(coin => CreateCoinModel.ConvertToEntity(coin, 0))
                                      .ToList();

        if (template.Coins.FirstOrDefault(c => c.CoinType == Domain.HubEntities.Enum.CoinType.Out) is Domain.HubEntities.Models.CreateOutCoinModel outCoinModel)
        {
            var withdrawOptions = await _coinService.GetWithdrawOptions(outCoinModel);
            var withdrawOptionGroups = await _coinService.GetWithdrawOptionGroups(outCoinModel);

            var outCoin = mappedCoins.OfType<OutCoin>()
                                     .FirstOrDefault(c => c.CoinType == Domain.HubEntities.Enum.CoinType.Out);

            if (outCoin != null)
            {
                if (withdrawOptions.Any())
                    outCoin.AddWithdrawOptions(withdrawOptions);

                if (withdrawOptionGroups.Any())
                    outCoin.AddWithdrawOptionGroups(withdrawOptionGroups);
            }
        }

        temp.Coins = mappedCoins;    

        var leaderboards = new List<LeaderboardRecord>();
        foreach(var leaderboard in template.Leaderboards)
        {
            var leadTemplate = new LeaderboardRecord
            {
                Title = leaderboard.Title,
                Description = leaderboard.Description,
                CreationDate = DateTime.Now,
                AnnouncementDate = leaderboard.AnnouncementDate,
                StartDate = leaderboard.StartDate,
                EndDate = leaderboard.EndDate,
                EventType = (Domain.LeaderBoradEntities.EventType)leaderboard.EventType,
                IsGenerated = leaderboard.IsGenerated,
                Status = (Domain.LeaderBoradEntities.LeaderboardRecordStatus)leaderboard.Status,
            };

            foreach (var prize in leaderboard.LeaderboardPrizes)
            {              
                leadTemplate.AddLeaderboardRecordPrizes(Random.Shared.Next(), prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
            }
            leaderboards.Add(leadTemplate);
            temp.Leaderboards = leaderboards;
        }

        var games = new List<GameConfigurationTemplate>();
        foreach (var item in template.Games)
        {
            var conf = await _gameTemplateService.CreateGameConfigurationTemplate(item.GameName, item.CreateGame);
            games.Add(conf);
            temp.Games = games;
        }

        await _promotionTemplateRepository.AddPromotionTemplateAsync(temp);

        return new ApplicationResult<bool> { Success = true };
    }

    public async Task<ApplicationResult<bool>> DeletePromotionTemplate(string id)
    {
        var template = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (template == null)
        {
            throw new NotFoundException("Template Not Found");
        }

        template.Delete();

        await _promotionTemplateRepository.UpdatePromotionTemplateAsync(id, template);

        return new ApplicationResult<bool> { Success = true };
    }

    public async Task<ApplicationResult<bool>> UpdatePromotionTemplate(UpdatePromotionTemplateDto update)
    {
        var template = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(update.Id);

        if (template == null || template.IsDeleted)
        {
            throw new NotFoundException($"template with the specified ID: [{update.Id}] was not found.");
        }
       
        await _promotionTemplateRepository.UpdatePromotionTemplateAsync(update.Id, template);

        return new ApplicationResult<bool> { Success = true };
    }
}