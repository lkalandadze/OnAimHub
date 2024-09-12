namespace GameLib.Application.Services.Abstract;

public interface IAuthService
{
    IEnumerable<string> GetCurrentPlayerSegmentIds();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}