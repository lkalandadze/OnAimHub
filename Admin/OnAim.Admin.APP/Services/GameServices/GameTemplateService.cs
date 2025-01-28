using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Services.GameServices;

public class GameTemplateService : IGameTemplateService
{
    private readonly IGameConfigurationTemplateRepository _gameConfigurationTemplateRepository;

    public GameTemplateService(IGameConfigurationTemplateRepository gameConfigurationTemplateRepository)
    {
        _gameConfigurationTemplateRepository = gameConfigurationTemplateRepository;
    }

    public async Task<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>> GetAllGameConfigurationTemplates(GameTemplateFilter filter)
    {
        var temps = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplates();

        if (filter?.Name != null)
        {
            temps = temps.Where(x => x.Game == filter.Name).ToList();
        }

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

        var result = temps.Select(x => new GameConfigurationTemplateDto
        {
           Id = x.Id,
            GameName = x.Game,
            Configuration = x.GetConfigurationAsJsonElement(),
        });

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = temps
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>
        {
            Success = true,
            Data = new PaginatedResult<GameConfigurationTemplateDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = result.ToList(),
            },
        };
    }

    public async Task<ApplicationResult<GameConfigurationTemplate>> GetGameConfigurationTemplateById(string id)
    {
        var coin = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("template Not Found");

        return new ApplicationResult<GameConfigurationTemplate> { Success = true, Data = coin };
    }

    public async Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(string gameName, object template)
    {
        var temp = new GameConfigurationTemplate
        {
            Game = gameName,
            Configuration = template.ToString(),
        };

        await _gameConfigurationTemplateRepository.AddGameConfigurationTemplateAsync(temp);

        return temp;
    }

    public async Task<ApplicationResult<bool>> DeleteGameConfigurationTemplate(string id)
    {
        var template = await _gameConfigurationTemplateRepository.DeleteGameConfigurationTemplateAsync(id);      

        return new ApplicationResult<bool> { Success = true };
    }
}