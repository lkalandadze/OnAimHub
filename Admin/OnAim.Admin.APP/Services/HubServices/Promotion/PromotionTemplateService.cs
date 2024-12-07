﻿using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public class PromotionTemplateService : IPromotionTemplateService
{
    private readonly IPromotionTemplateRepository _promotionTemplateRepository;
    private readonly ICoinService _coinService;
    private readonly ICoinTemplateService _coinTemplateService;
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;
    private readonly IGameTemplateService _gameTemplateService;

    public PromotionTemplateService(IPromotionTemplateRepository promotionTemplateRepository,
        ICoinService coinService,
        ICoinTemplateService coinTemplateService,
        ILeaderboardTemplateService leaderboardTemplateService,
        IGameTemplateService gameTemplateService
        )
    {
        _promotionTemplateRepository = promotionTemplateRepository;
        _coinService = coinService;
        _coinTemplateService = coinTemplateService;
        _leaderboardTemplateService = leaderboardTemplateService;
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult> GetAllTemplates()
    {
        var temps = await _promotionTemplateRepository.GetPromotionTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var coin = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

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

        var coinTemplates = new List<CoinTemplate>();
        foreach (var item in template.Coins)
        {
            var coinTemplate = await _coinTemplateService.CreateCoinTemplate(item);
            coinTemplates.Add(coinTemplate);
            temp.UpdateCoins(coinTemplates);
        }  
        
        var leaderboards = new List<LeaderboardTemplate>();
        foreach(var leaderboard in template.Leaderboards)
        {
            var leadTemp = await _leaderboardTemplateService.CreateLeaderboardTemplate(leaderboard);
            leaderboards.Add(leadTemp);
            temp.Leaderboards = leaderboards;
        }

        var games = new List<GameConfigurationTemplate>();
        foreach (var item in template.Games)
        {
            var conf = await _gameTemplateService.CreateGameConfigurationTemplate(item);
            games.Add(conf);
            temp.Games = games;
        }

        await _promotionTemplateRepository.AddPromotionTemplateAsync(temp);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeletePromotionTemplate(string CoinTemplateId)
    {
        //var coinTemplate = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(CoinTemplateId);

        //if (coinTemplate == null)
        //{
        //    throw new NotFoundException("Coin Template Not Found");
        //}

        //coinTemplate.Delete();

        //await _coinRepository.UpdateCoinTemplateAsync(CoinTemplateId, coinTemplate);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdatePromotionTemplate(UpdateCoinTemplateDto update)
    {
        //var coinTemplate = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(update.Id);

        //if (coinTemplate == null || coinTemplate.IsDeleted)
        //{
        //    throw new NotFoundException($"Coin template with the specified ID: [{update.Id}] was not found.");
        //}

        return new ApplicationResult { Success = true };
    }
}

public record CreatePromotionTemplate(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreateCoinTemplateDto>? Coins,
    IEnumerable<CreateLeaderboardTemplateDto>? Leaderboards,
    IEnumerable<CreateGameConfigurationTemplateDto> Games
    );