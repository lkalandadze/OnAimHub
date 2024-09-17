using Hub.Application.Models.Progress;

namespace Hub.Application.Services.Abstract;

public interface IPlayerProgressService
{
    Task InsertOrUpdateProgressesAsync(PlayerProgressGetModel model, int playerId);
}