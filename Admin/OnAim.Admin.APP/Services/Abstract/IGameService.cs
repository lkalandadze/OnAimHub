using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IGameService
{
    Task<List<GameListDtoItem>> GetAll();
    Task<object> GetConfigurations();
    Task<object> GetConfiguration(int id);
    Task<string> GetGame();
}
