using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IAuthService
{
    Player GetCurrentPlayer();

    public IEnumerable<PlayerSegment> GetCurrentPlayerSegments();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}