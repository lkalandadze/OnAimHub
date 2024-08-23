using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IAuthService
{
    Player GetCurrentPlayer();

    int GetCurrentPlayerSegmentId();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}