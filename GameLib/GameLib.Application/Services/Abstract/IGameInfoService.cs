using GameLib.Application.Models.Game;

namespace GameLib.Application.Services.Abstract;

public interface IGameInfoService
{
    Task<GetGameShortInfoModel> GetGameShortInfo();
}