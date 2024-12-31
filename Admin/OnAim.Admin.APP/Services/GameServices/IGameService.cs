using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameService
{
    Task<object> GetAll(FilterGamesDto? filter);
    Task<bool> GameStatus(string name);
    Task<object> ActivateGame(string name);
    Task<object> DeactivateGame(string name);
    Task<object> GetConfigurations(string name, int promotionId);
    Task<object> GetConfiguration(string name, int id);
    Task<string> GetGame(string name);
    Task<object> GetConfigurationMetadata(string name);
    Task<object> CreateConfiguration(string name, GameConfigurationDto configurationJson);
    Task<object> UpdateConfiguration(string name, GameConfigurationDto configurationJson);
    Task<object> ActivateConfiguration(string name, int id);
    Task<object> DeactivateConfiguration(string name, int id);
}
