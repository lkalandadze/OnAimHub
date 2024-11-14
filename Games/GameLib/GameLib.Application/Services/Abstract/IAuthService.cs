namespace GameLib.Application.Services.Abstract;

public interface IAuthService
{
    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}