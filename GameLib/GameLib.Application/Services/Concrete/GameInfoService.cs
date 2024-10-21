using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions.Repository;

namespace GameLib.Application.Services.Concrete;

public class GameInfoService : IGameInfoService
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly GameSettings _gameSettings;

    public GameInfoService(ISegmentRepository segmentRepository, IGameConfigurationRepository configurationRepository, GameSettings gameSettings)
    {
        _segmentRepository = segmentRepository;
        _configurationRepository = configurationRepository;
        _gameSettings = gameSettings;
    }

    public async Task<GetGameShortInfoModel> GetGameShortInfo()
    {
        var configurations = await _configurationRepository.QueryAsync();
        var segments = await _segmentRepository.QueryAsync();

        return new GetGameShortInfoModel
        {
            Status = _gameSettings.IsActive.Value,
            Description = _gameSettings.Description.Value,
            ConfigurationCount = configurations.Count(),
            Segments = segments.Select(s => s.Id),
        };
    }
}