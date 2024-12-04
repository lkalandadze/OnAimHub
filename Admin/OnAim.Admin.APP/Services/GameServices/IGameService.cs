using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameService
{
    Task<List<GameListDtoItem>> GetAll();
    Task<object> GetConfigurations();
    Task<object> GetConfiguration(int id);
    Task<string> GetGame();
    Task<object> GetConfigurationMetadata();
    Task<object> CreateConfiguration(string configurationJson);
    Task<object> UpdateConfiguration(string configurationJson);
    Task<object> ActivateConfiguration(int id);
    Task<object> DeactivateConfiguration(int id);
    Task<object> GetPrizeTypes();
    Task<object> GetPrizeTypeById(int id);
    Task<object> CreatePrizeType(CreatePrizeTypeDto createPrize);
    Task<object> UpdatePrizeType(int id, CreatePrizeTypeDto typeDto);
}
