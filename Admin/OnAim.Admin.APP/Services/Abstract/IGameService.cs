using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Game;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IGameService
{
    Task<ApplicationResult<List<GameListDto>>> GetAll();
    Task<object> GetGame(int id);
    Task<object> GetConfiguration(int id);
}
