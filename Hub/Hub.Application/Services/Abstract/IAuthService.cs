using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IAuthService
{
    Player GetCurrentPlayer();

    public List<string> GetCurrentPlayerSegmentIds();

    //int GetCurrentPlayerSegmentIds();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}