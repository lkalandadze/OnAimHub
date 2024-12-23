using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Domain.GameEntities;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameService
{
    Task<string> GetAll(FilterGamesDto? filter);
    Task<object> GetConfigurations();
    Task<object> GetConfiguration(int id);
    Task<string> GetGame();
    Task<object> GetConfigurationMetadata();
    Task<object> CreateConfiguration(string gameName, GameConfigurationDto configurationJson);
    Task<object> UpdateConfiguration(string gameName, GameConfigurationDto configurationJson);
    Task<object> ActivateConfiguration(int id);
    Task<object> DeactivateConfiguration(int id);
    Task<object> GetPrizeTypes();
    Task<object> GetPrizeTypeById(int id);
    Task<object> CreatePrizeType(CreatePrizeTypeDto createPrize);
    Task<object> UpdatePrizeType(int id, CreatePrizeTypeDto typeDto);
}
