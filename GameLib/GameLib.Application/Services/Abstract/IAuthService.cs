namespace GameLib.Application.Services.Abstract;

public interface IAuthService
{
    IEnumerable<int> GetCurrentPlayerSegmentIds();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}