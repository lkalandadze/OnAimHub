using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.GameServices;

public class GameTemplateService : IGameTemplateService
{
    private readonly IGameConfigurationTemplateRepository _gameConfigurationTemplateRepository;

    public GameTemplateService(IGameConfigurationTemplateRepository gameConfigurationTemplateRepository)
    {
        _gameConfigurationTemplateRepository = gameConfigurationTemplateRepository;
    }

    public async Task<ApplicationResult> GetAllGameConfigurationTemplates(BaseFilter filter)
    {
        var temps = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplates();

        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = temps
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<GameConfigurationTemplate>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult> GetGameConfigurationTemplateById(string id)
    {
        var coin = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("template Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(CreateGameConfigurationTemplateDto coinTemplate)
    {
        var temp = new GameConfigurationTemplate
        {
            Name = coinTemplate.Name,
            Value = coinTemplate.Value,
            IsActive = coinTemplate.IsActive,
            Prices = coinTemplate.Prices.Select(x => new Price
            {
                Value = x.Value,
                Multiplier = x.Multiplier,
                CoinId = x.CoinId,
            }).ToList(),
            Rounds = coinTemplate.Rounds.Select(xx => new Round
            {
                Sequence = xx.Sequence,
                Name = xx.Name,
                NextPrizeIndex = xx.NextPrizeIndex,
                ConfigurationId = xx.ConfigurationId,
                Prizes = xx.Prizes.Select(xxx => new Prize
                {
                    Value = xxx.Value,
                    PrizeGroupId = xxx.PrizeGroupId,
                    PrizeTypeId = xxx.PrizeTypeId,
                    Probability = xxx.Probability,
                    Name = xxx.Name,
                    WheelIndex = xxx.WheelIndex,
                }).ToList(),
            }).ToList(),
        };

        await _gameConfigurationTemplateRepository.AddGameConfigurationTemplateAsync(temp);

        return temp;
    }

    public async Task<ApplicationResult> DeleteGameConfigurationTemplate(string id)
    {
        var template = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplateByIdAsync(id);

        if (template == null)
        {
            throw new NotFoundException("Template Not Found");
        }

        template.Delete();

        await _gameConfigurationTemplateRepository.UpdateGameConfigurationTemplateAsync(id, template);

        return new ApplicationResult { Success = true };
    }
}