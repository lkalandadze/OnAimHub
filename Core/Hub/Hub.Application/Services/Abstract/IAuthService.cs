using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IAuthService
{
    Player GetCurrentPlayer();

    public IEnumerable<string> GetCurrentPlayerSegments();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}